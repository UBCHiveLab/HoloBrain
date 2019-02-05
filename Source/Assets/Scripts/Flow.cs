using HoloToolkit.Examples.InteractiveElements; using System; using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.UI; using System.Linq; using HoloToolkit.Unity.InputModule; using UnityEngine.SceneManagement;

public class Flow : MonoBehaviour {      public GameObject NextButton;     public AudioSource audio;     public string nextScene = "Main";      private List<Transform> PagesInChapters;     private IEnumerator PagesInChaptersEnumerator;     private Transform CurrentPage;      // Use this for initialization     void Start()     {         if (this.GetComponent<AudioSource>() != null)
        {
            audio = this.GetComponent<AudioSource>();
        }          if (NextButton != null)         {             NextButton.GetComponent<Button>().onClick.AddListener(ShowNextChapterPages);
            //NextButton.GetComponent<Interactive>().OnSelectEvents.AddListener(ShowNextChapterPages);
        }          InitializePagesInChaptersAndItsEnumerator();         ShowNextChapterPages();     }      private void InitializePagesInChaptersAndItsEnumerator()     {         PagesInChapters = new List<Transform>();          foreach (Transform page in this.gameObject.transform)         {             PagesInChapters.Add(page);         }          TurnOffAllRenderer(PagesInChapters);         PagesInChaptersEnumerator = PagesInChapters.GetEnumerator();     }      public void ShowNextChapterPages()     {         if (PagesInChaptersEnumerator.MoveNext() == false || PagesInChaptersEnumerator == null)         {             //PagesInChaptersEnumerator.Reset();             //PagesInChaptersEnumerator.MoveNext();             SceneManager.LoadScene(nextScene);         }

        TurnOffAllRenderer(PagesInChapters);         CurrentPage = (Transform)PagesInChaptersEnumerator.Current;
        StartFirstPageTimer(CurrentPage);
        TurnOnAllRenderer(CurrentPage);     }      private void StartFirstPageTimer(Transform CurrentPage) {
        if (CurrentPage == PagesInChapters.First())
        {
            StartCoroutine(FirstPageTimer());
        }
    }

    IEnumerator FirstPageTimer()
    {
        //float length = audio.clip.length;
        //float buffer = 5f;
        //yield return new WaitForSeconds(length + buffer);
        yield return new WaitForSeconds(10);
        ShowNextChapterPages();     }      private void TurnOnAllRenderer(Transform CurrentPage)     {         CurrentPage.gameObject.SetActive(true);     }      private void TurnOffAllRenderer(List<Transform> pagesInCurrentChapter)     {         foreach (Transform r in pagesInCurrentChapter)         {             r.gameObject.SetActive(false);         }     }  }