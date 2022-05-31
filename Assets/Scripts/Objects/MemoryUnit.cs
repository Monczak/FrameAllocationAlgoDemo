using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryUnit : MonoBehaviour
{
    public PageReplacementAlgorithm pageReplacementAlgorithm;
    public PageReplacementAlgorithmType pageReplacementAlgorithmType;

    public FrameAllocator frameAllocationAlgorithm;
    public FrameAllocatorType frameAllocationAlgorithmType;

    private MemoryUnitState[] states;

    public MemoryPage[] pages;
    private MemoryPageRenderer pageRenderer;

    private int pageFaults;

    public Transform @base, pagesTransform;
    public TMP_Text algorithmText, pageFaultsText;
    public RectTransform progressBarCanvas;
    public Image progressBar;

    private SimulationSettings cachedSettings;
    private int statesComputed;

    private CancellationTokenSource cancellationTokenSource;

    private void Awake()
    {
        pageRenderer = GetComponent<MemoryPageRenderer>();

        ClearPages();

        cancellationTokenSource = new CancellationTokenSource();
        statesComputed = -1;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateProgressBar();

        UpdateCurrentState();

        UpdateTexts();
        pageRenderer.RenderPages(pages);
    }

    private void UpdateCurrentState()
    {
        int time = Mathf.FloorToInt(SimulationManager.Instance.currentTime);
        pages = states[Mathf.Min(statesComputed - 1, time)].Pages;
        pageFaults = states[Mathf.Min(statesComputed - 1, time)].PageFaults;
    }

    private void UpdateTexts()
    {
        pageFaultsText.text = pageFaults.ToString();
    }

    public void SetProperties(FrameAllocator frameAllocationAlgorithm, Vector3 size)
    {
        this.frameAllocationAlgorithm = frameAllocationAlgorithm;
        pageReplacementAlgorithm = new LRUAlgorithm(frameAllocationAlgorithm);

        pageReplacementAlgorithmType = pageReplacementAlgorithm.AlgorithmType;
        frameAllocationAlgorithmType = frameAllocationAlgorithm == null ? FrameAllocatorType.Equal : frameAllocationAlgorithm.AlgorithmType;

        algorithmText.text = frameAllocationAlgorithm == null ? "A" : frameAllocationAlgorithm.AlgorithmName;

        @base.localScale = size;
        pagesTransform.localScale = size - new Vector3(0.5f, 0, 0.5f);

        algorithmText.rectTransform.localPosition = new Vector3(0, 0.001f, (size.z + algorithmText.rectTransform.sizeDelta.y) / 2 + 0.2f);
        pageFaultsText.rectTransform.localPosition = new Vector3(0, 0.001f, -(size.z + pageFaultsText.rectTransform.sizeDelta.y) / 2 - 0.2f);
    }

    public void ClearPages()
    {
        pages = new MemoryPage[cachedSettings.memorySize];
        NullifyPages(pages);
    }

    private void NullifyPages(MemoryPage[] pages)
    {
        for (int i = 0; i < pages.Length; i++)
            pages[i] = MemoryPage.NullPage;
    }

    public void Simulate(Queue<Request> requests)
    {
        cachedSettings = SimulationManager.Instance.simulationSettings;

        ClearPages();
        pageRenderer.CreateTexture(pages.Length);

        states = new MemoryUnitState[requests.Count + 1];
        states[0] = new MemoryUnitState
        {
            Pages = new MemoryPage[cachedSettings.memorySize],
            PageFaults = 0,
        };
        NullifyPages(states[0].Pages);

        if (statesComputed != -1 && statesComputed - 1 != cachedSettings.sequenceLength)
            cancellationTokenSource.Cancel();

        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        List<Process> processes = new List<Process>(SimulationManager.Instance.processes);

        Task.Factory.StartNew(() =>
        {
            IEnumerator<MemoryUnitState> stateGenerator = pageReplacementAlgorithm.Run(requests, cachedSettings);
            statesComputed = 1;
            while (stateGenerator.MoveNext())
            {
                states[statesComputed++] = stateGenerator.Current;

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }).ContinueWith(t =>
        {
            OnSimulationDone(t);
        });
    }

    private void OnSimulationDone(Task task)
    {
        if (task.IsFaulted)
        {
            string message = $"{frameAllocationAlgorithm.AlgorithmName} failed: ";
            foreach (var e in task.Exception?.InnerExceptions)
            {
                message += $"{e.Message}\n{e.StackTrace}\n";
            }
            Debug.LogError(message);
        }
        OnSimulationFinished?.Invoke(this);
    }

    public delegate void OnSimulationFinishedDelegate(MemoryUnit unit);
    public event OnSimulationFinishedDelegate OnSimulationFinished;

    public void UpdateProgressBar()
    {
        progressBarCanvas.sizeDelta = new Vector2(@base.transform.localScale.x, @base.transform.localScale.z);
        progressBar.fillAmount = 1 - (float)statesComputed / cachedSettings.sequenceLength;
    }
}
