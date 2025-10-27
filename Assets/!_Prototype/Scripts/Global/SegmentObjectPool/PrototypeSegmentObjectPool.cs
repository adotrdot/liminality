using UnityEngine;
using UnityEngine.AI;

public class PrototypeSegmentObjectPool : MonoBehaviour
{
    #region Fields and Properties

    

    #endregion

    #region Monobehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Public Methods

    public GameObject GetBeginningPathSegment()
    {
        return GetPathSegment(PrototypeSegmentType.BeginningPathSegment);
    }

    public GameObject GetStraightPathSegment()
    {
        return GetPathSegment(PrototypeSegmentType.StraightPathSegment);
    }

    public GameObject GetBranchingPathSegment()
    {
        return GetPathSegment(PrototypeSegmentType.BranchingPathSegment);
    }

    public void ReturnPathSegmentToPool(GameObject segment)
    {
        segment.SetActive(false);
        segment.transform.SetParent(transform);
    }
    
    #endregion

    #region Private and protected methods
    
    private GameObject GetPathSegment(PrototypeSegmentType segmentType)
    {
        GameObject segment = null;
        string tag;

        switch (segmentType)
        {
            case PrototypeSegmentType.BeginningPathSegment:
                tag = "BeginningPathSegment";
                break;
            case PrototypeSegmentType.StraightPathSegment:
                tag = "StraightPathSegment";
                break;
            case PrototypeSegmentType.BranchingPathSegment:
                tag = "BranchingPathSegment";
                break;
            default:
                tag = "";
                break;
        }

        if (string.IsNullOrEmpty(tag))
        {
            Debug.LogWarning("Invalid segment type specified.");
            return null;
        }
        
        // Get the specified path segment from pool
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag(tag))
            {
                segment = child.gameObject;
                break;
            }

        }

        if (segment == null)
            Debug.LogWarning("No specified path segment is available in the pool.");

        return segment;
    }

    #endregion
}
