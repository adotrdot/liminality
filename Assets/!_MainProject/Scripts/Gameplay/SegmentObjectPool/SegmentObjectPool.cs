using UnityEngine;

/// <summary>
/// A class to manage the level segment object pool.
/// </summary>
public class SegmentObjectPool : MonoBehaviour
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
        return GetPathSegment(SegmentType.BeginningPathSegment);
    }

    public GameObject GetStraightPathSegment()
    {
        return GetPathSegment(SegmentType.StraightPathSegment);
    }

    public GameObject GetBranchingPathSegment()
    {
        return GetPathSegment(SegmentType.BranchingPathSegment);
    }

    public void ReturnPathSegmentToPool(GameObject segment)
    {
        segment.SetActive(false);
        segment.transform.SetParent(transform);
    }

    #endregion

    #region Private and protected methods

    private GameObject GetPathSegment(SegmentType segmentType)
    {
        GameObject segment = null;
        string tag;

        switch (segmentType)
        {
            case SegmentType.BeginningPathSegment:
                tag = "BeginningPathSegment";
                break;
            case SegmentType.StraightPathSegment:
                tag = "StraightPathSegment";
                break;
            case SegmentType.BranchingPathSegment:
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