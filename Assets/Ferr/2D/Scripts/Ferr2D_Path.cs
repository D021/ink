using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A generic path with lots of helper functions. Should be useful for all sorts of things!
/// </summary>
[AddComponentMenu("Ferr2D/Path")]
public class Ferr2D_Path : MonoBehaviour
{
    #region Fields and properties
    /// <summary>
    /// If the path should connect at the ends! Influences interpolation, especially for normals.
    /// </summary>
	public bool           closed    = false;
    /// <summary>
    /// If you really want access to these, you should call GetVerts
    /// </summary>
	public List<Vector2>  pathVerts = new List<Vector2>();
    /// <summary>
    /// Returns the number of vertices in the path
    /// </summary>
    public int Count { get { return pathVerts.Count; } }
    #endregion

    #region Methods
	/// <summary>
	/// Creates a Path object from a JSON string.
	/// </summary>
	/// <param name="aJSON">A JSON string, gets parsed and sent to FromJSON(Ferr_JSONValue)</param>
	public void FromJSON(string aJSON) {
		FromJSON(Ferr_JSON.Parse(aJSON));
	}
	/// <summary>
	/// Creates a Path object from a JSON value object.
	/// </summary>
	/// <param name="aJSON">A JSON object with path data!</param>
	public void           FromJSON(Ferr_JSONValue aJSON) {
		closed         = aJSON["closed", false         ];
		pathVerts      = new List<Vector2>();
		object[] verts = aJSON["verts",  new object[]{}];
		
		for (int i = 0; i < verts.Length; i++) {
			if (verts[i] is Ferr_JSONValue) {
				Ferr_JSONValue v = verts[i] as Ferr_JSONValue;
				pathVerts.Add(new Vector2(v[0,0f], v[1,0f]));
			}
		}
	}
	/// <summary>
	/// Creates a JSON value object from this path.
	/// </summary>
	/// <returns>JSON Value object, can put it into a larger JSON object, or just ToString it.</returns>
	public Ferr_JSONValue ToJSON  () {
		Ferr_JSONValue result = new Ferr_JSONValue();
		result["closed"] = closed;
		
		object[] list = new object[pathVerts.Count];
		for (int i = 0; i < pathVerts.Count; i++) {
			Ferr_JSONValue vert = new Ferr_JSONValue();
			vert[0] = pathVerts[i].x;
			vert[1] = pathVerts[i].y;
			list[i] = vert;
		}
		
		result["verts"] = list;
		
		return result;
	}

    /// <summary>
    /// Moves the object location to the center of the path verts. Also offsets the path locations to match.
    /// </summary>
    public void ReCenter        ()
    {
        Vector2 center = Vector2.zero;
        for (int i = 0; i < pathVerts.Count; i++)
        {
            center += pathVerts[i];
        }
        center = center / pathVerts.Count + new Vector2(transform.position.x, transform.position.y);
        Vector2 offset = center - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        for (int i = 0; i < pathVerts.Count; i++)
        {
            pathVerts[i] -= offset;
        }
        gameObject.transform.position = new Vector3(center.x, center.y, gameObject.transform.position.z);

        UpdateDependants();
    }
    /// <summary>
    /// Updates all other component on this GameObject that implement the Ferr2DT_IPath interface.
    /// </summary>
    public void UpdateDependants()
    {
        Component[] coms = gameObject.GetComponents(typeof(Ferr2D_IPath));
        for (int i = 0; i < coms.Length; i++)
        {
            (coms[i] as Ferr2D_IPath).RecreatePath();
        }
    }
    /// <summary>
    /// Adds a vertex to the end of the path.
    /// </summary>
    /// <param name="aPoint">The vertex to add!</param>
    public void Add             (Vector2 aPoint)
    {
        pathVerts.Add(aPoint);
    }
    /// <summary>
    /// Gets the index of the path point that starts the closest line segment to the specified point.
    /// </summary>
    /// <param name="aPoint">The point to check from.</param>
    /// <returns>Index of the first point in the line segment, the other point would be Index+1</returns>
    public int  GetClosestSeg   (Vector2 aPoint)
    {
        if (pathVerts.Count <= 0) return -1;

        float dist  = float.MaxValue;
        int   seg   = -1;
		int   count = closed ? pathVerts.Count : pathVerts.Count-1;
        for (int i = 0; i < count; i++)
        {
            int next = i == pathVerts.Count -1 ? 0 : i + 1;
            Vector2 pt    = GetClosetPointOnLine(pathVerts[i], pathVerts[next], aPoint, true);
            float   tDist = (aPoint - pt).SqrMagnitude();
            if (tDist < dist)
            {
                dist = tDist;
                seg  = i;
            }
        }
        if (!closed)
        {
            float tDist = (aPoint - pathVerts[pathVerts.Count - 1]).SqrMagnitude();
            if (tDist <= dist)
            {
                seg = pathVerts.Count - 1;
            }
            tDist = (aPoint - pathVerts[0]).SqrMagnitude();
            if (tDist <= dist)
            {
                seg = pathVerts.Count - 1;
            }
        }
        return seg;
    }

    /// <summary>
    /// Gets a -copy- of the vertices in the path.
    /// </summary>
    /// <param name="aSmoothed">Should the vertices be smoothed first?</param>
    /// <param name="aSplitDistance">If they're smoothed, how far apart should each smooth split be?</param>
    /// <param name="aSplitCorners">Should we make corners sharp? Sharp corners don't get smoothed.</param>
    /// <returns>A new list of vertices!</returns>
    public List<Vector2> GetVerts        (bool aSmoothed, float aSplitDistance, bool aSplitCorners)
    {
        if (aSmoothed) return GetVertsSmoothed(aSplitDistance, aSplitCorners, false);
        else return GetVertsRaw();
    }
    /// <summary>
    /// Don't care about smoothing? If aSmoothed is false, GetVerts calls this method.
    /// </summary>
    /// <returns>Just a plain old copy of pathVerts.</returns>
    public List<Vector2> GetVertsRaw     ()
    {
        List<Vector2> result = new List<Vector2>(pathVerts);
        return result;
    }
    /// <summary>
    /// Gets a copy of the vertices that's smoothed.
    /// </summary>
    /// <param name="aSplitDistance">If they're smoothed, how far apart should each smooth split be?</param>
    /// <param name="aSplitCorners">Should we make corners sharp? Sharp corners don't get smoothed.</param>
    /// <returns>A copy of the smoothed data.</returns>
    public List<Vector2> GetVertsSmoothed(float aSplitDistance, bool aSplitCorners, bool aInverted)
    {
        List<Vector2> result = new List<Vector2>();
        if (aSplitCorners)
        {
            List<Ferr2DT_TerrainDirection> dirs;
            List<List<Vector2>> segments = GetSegments(pathVerts, out dirs);
            if (closed) CloseEnds(ref segments, ref dirs, aSplitCorners, aInverted);
            if (segments.Count > 1) {
                for (int i = 0; i < segments.Count; i++) {
                    segments[i] = SmoothSegment(segments[i], aSplitDistance, false);
                    if (i != 0 && segments[i].Count > 0) segments[i].RemoveAt(0);
                    result.AddRange(segments[i]);
                }
            } else {
                result = SmoothSegment(pathVerts, aSplitDistance, closed);
                if (closed) result.Add(pathVerts[0]);
            }
        }
        else {
            result = SmoothSegment(pathVerts, aSplitDistance, closed);
            if (closed) result.Add(pathVerts[0]);
        }
        return result;
    }
    #endregion

    #region Static Methods
    /// <summary>
    /// Gets the normal at the specified path index.
    /// </summary>
    /// <param name="aSegment">The list of vertices used to calculate the normal.</param>
    /// <param name="i">Index of the vertex to get the normal of.</param>
    /// <param name="aClosed">Should we interpolate at the edges of the path as though it was closed?</param>
    /// <returns>A normalized normal!</returns>
    public  static Vector2   GetNormal        (List<Vector2> aSegment, int i, bool  aClosed) {
		if (aSegment.Count < 2) return Vector2.up;
		Vector2 curr = aClosed && i == aSegment.Count - 1 ? aSegment[0] : aSegment[i];

        // get the vertex before the current vertex
		Vector2 prev = Vector2.zero;
		if (i-1 < 0) {
			if (aClosed) {
				prev = aSegment[aSegment.Count-2];
			} else {
				prev = curr - (aSegment[i+1]-curr);
			}
		} else {
			prev = aSegment[i-1];
		}
		
        // get the vertex after the current vertex
		Vector2 next = Vector2.zero;
		if (i+1 > aSegment.Count-1) {
			if (aClosed) {
				next = aSegment[1];
			} else {
				next = curr - (aSegment[i-1]-curr);
			}
		} else {
			next = aSegment[i+1];
		}

		prev = prev - curr;
		next = next - curr;
		
		prev.Normalize ();
		next.Normalize ();
		
		prev = new Vector2(-prev.y, prev.x);
		next = new Vector2(next.y, -next.x);
		
		Vector2 norm = (prev + next) / 2;
		norm.Normalize();

		return norm;
	}
    /// <summary>
    /// Gets the normal at the specified path index using cubic interpolation for smoothing.
    /// </summary>
    /// <param name="aSegment">The list of vertices used to calculate the normal.</param>
    /// <param name="i">Index of the vertex to start from.</param>
    /// <param name="aPercentage">How far between this vertex and the next should we look?</param>
    /// <param name="aClosed">Should we interpolate at the edges of the path as though it was closed?</param>
    /// <returns>A normalized cubic interpolated normal!</returns>
    public  static Vector2   CubicGetNormal   (List<Vector2> aSegment, int i, float aPercentage, bool aClosed)
    {
        Vector2 p1 = CubicGetPt(aSegment, i, aPercentage, aClosed);
        Vector2 p2 = CubicGetPt(aSegment, i, aPercentage+ 0.01f, aClosed);
        Vector2 dir = p2 - p1;
        dir.Normalize();
        return new Vector2(dir.y, -dir.x);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="aSegment">The list of vertices used to calculate the point.</param>
    /// <param name="i">Index of the vertex to start from.</param>
    /// <param name="aPercentage">How far between this vertex and the next should we look?</param>
    /// <param name="aClosed">Should we interpolate at the edges of the path as though it was closed?</param>
    /// <returns>A cubic interpolated point.</returns>
    public  static Vector2   CubicGetPt       (List<Vector2> aSegment, int i, float aPercentage, bool aClosed) 
    {
        int a1 = aClosed ? i - 1 < 0 ? aSegment.Count-1 : i - 1 : Mathf.Clamp(i - 1, 0, aSegment.Count - 1);
        int a2 = i;
        int a3 = aClosed ? (i + 1) % (aSegment.Count - 1) : Mathf.Clamp(i + 1, 0, aSegment.Count - 1);
        int a4 = aClosed ? (i + 2) % (aSegment.Count - 1) : Mathf.Clamp(i + 2, 0, aSegment.Count - 1);

        return new Vector2(
            Cubic(aSegment[a1].x, aSegment[a2].x, aSegment[a3].x, aSegment[a4].x, aPercentage),
            Cubic(aSegment[a1].y, aSegment[a2].y, aSegment[a3].y, aSegment[a4].y, aPercentage));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="aSegment">The list of vertices used to calculate the normal.</param>
    /// <param name="i">Index of the vertex to start from.</param>
    /// <param name="aPercentage">How far between this vertex and the next should we look?</param>
    /// <param name="aClosed">Should we interpolate at the edges of the path as though it was closed?</param>
    /// <returns>A normalized Hermite interpolated normal!</returns>
    public  static Vector2   HermiteGetNormal (List<Vector2> aSegment, int i, float aPercentage, bool aClosed, float aTension = 0, float aBias = 0)
    {
        Vector2 p1 = HermiteGetPt(aSegment, i, aPercentage,         aClosed, aTension, aBias);
        Vector2 p2 = HermiteGetPt(aSegment, i, aPercentage + 0.01f, aClosed, aTension, aBias);
        Vector2 dir = p2 - p1;
        dir.Normalize();
        return new Vector2(dir.y, -dir.x);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="aSegment">The list of vertices used to calculate the point.</param>
    /// <param name="i">Index of the vertex to start from.</param>
    /// <param name="aPercentage">How far between this vertex and the next should we look?</param>
    /// <param name="aClosed">Should we interpolate at the edges of the path as though it was closed?</param>
    /// <returns>A Hermite interpolated point.</returns>
    public  static Vector2   HermiteGetPt     (List<Vector2> aSegment, int i, float aPercentage, bool aClosed, float aTension = 0, float aBias = 0)
    {
        int a1 = aClosed ? i - 1 < 0 ? aSegment.Count - 2 : i - 1 : Mathf.Clamp(i - 1, 0, aSegment.Count - 1);
        int a2 = i;
        int a3 = aClosed ? (i + 1) % (aSegment.Count) : Mathf.Clamp(i + 1, 0, aSegment.Count - 1);
        int a4 = aClosed ? (i + 2) % (aSegment.Count) : Mathf.Clamp(i + 2, 0, aSegment.Count - 1);

        return new Vector2(
            Hermite(aSegment[a1].x, aSegment[a2].x, aSegment[a3].x, aSegment[a4].x, aPercentage, aTension, aBias),
            Hermite(aSegment[a1].y, aSegment[a2].y, aSegment[a3].y, aSegment[a4].y, aPercentage, aTension, aBias));
    }

    private static float Cubic  (float v1, float v2, float v3, float v4, float aPercentage)
    {
        float percentageSquared = aPercentage * aPercentage;
        float a1 = v4 - v3 - v1 + v2;
        float a2 = v1 - v2 - a1;
        float a3 = v3 - v1;
        float a4 = v2;

        return (a1 * aPercentage * percentageSquared + a2 * percentageSquared + a3 * aPercentage + a4);
    }
    private static float Linear (float v1, float v2,                     float aPercentage)
    {
        return v1 + (v2 - v1) * aPercentage;
    }
    private static float Hermite(float v1, float v2, float v3, float v4, float aPercentage, float aTension, float aBias)
    {
        float mu2 = aPercentage * aPercentage;
        float mu3 = mu2 * aPercentage;
        float m0 = (v2 - v1) * (1 + aBias) * (1 - aTension) / 2;
        m0 += (v3 - v2) * (1 - aBias) * (1 - aTension) / 2;
        float m1 = (v3 - v2) * (1 + aBias) * (1 - aTension) / 2;
        m1 += (v4 - v3) * (1 - aBias) * (1 - aTension) / 2;
        float a0 = 2 * mu3 - 3 * mu2 + 1;
        float a1 = mu3 - 2 * mu2 + aPercentage;
        float a2 = mu3 - mu2;
        float a3 = -2 * mu3 + 3 * mu2;

        return (a0 * v2 + a1 * m0 + a2 * m1 + a3 * v3);
    }

    /// <summary>
    /// Gets the direction enum of a line segment.
    /// </summary>
    /// <param name="aOne">First vertex in the line segment.</param>
    /// <param name="aTwo">Second vertex in the line segment.</param>
    /// <returns>Direction enum!</returns>
    public static Ferr2DT_TerrainDirection GetDirection (Vector2 aOne, Vector2 aTwo)
    {
        Vector2 dir = aOne - aTwo;
        dir = new Vector2(-dir.y, dir.x);
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x < 0) return Ferr2DT_TerrainDirection.Left;
            else return Ferr2DT_TerrainDirection.Right;
        }
        else
        {
            if (dir.y < 0) return Ferr2DT_TerrainDirection.Bottom;
            else return Ferr2DT_TerrainDirection.Top;
        }
    }
    /// <summary>
    /// Gets the direction enum of a line segment, invertable!
    /// </summary>
    /// <param name="aSegment">list of vertices to pick from</param>
    /// <param name="i">First vertex to use as the ine segment, next is i+1 (or i-1 if i+1 is outside the array)</param>
    /// <param name="aInvert">Flip the direction around?</param>
    /// <returns>Direction enum!</returns>
    public static Ferr2DT_TerrainDirection GetDirection (List<Vector2> aSegment, int i, bool aInvert, bool aClosed = false, List<Ferr2DT_TerrainDirection> aOverrides = null)
    {
        if (aSegment.Count <= 0) return Ferr2DT_TerrainDirection.Top;

        int next = i+1;
        if (i < 0) {
            if (aClosed) {
                i    = aSegment.Count-2;
                next = 0;
            } else {
                i=0;
                next = 1;
            }
        }

        if (aOverrides != null && aOverrides.Count >= aSegment.Count && aOverrides[i] != Ferr2DT_TerrainDirection.None) return aOverrides[i];

        Vector2 dir = aSegment[next > aSegment.Count-1? (aClosed? aSegment.Count-1 : i-1) : next] - aSegment[i];
        dir         = new Vector2(-dir.y, dir.x);
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x < 0) return aInvert ? Ferr2DT_TerrainDirection.Right : Ferr2DT_TerrainDirection.Left;
            else           return aInvert ? Ferr2DT_TerrainDirection.Left  : Ferr2DT_TerrainDirection.Right;
        }
        else
        {
            if (dir.y < 0) return aInvert ? Ferr2DT_TerrainDirection.Top    : Ferr2DT_TerrainDirection.Bottom;
            else           return aInvert ? Ferr2DT_TerrainDirection.Bottom : Ferr2DT_TerrainDirection.Top;
        }
    }
    /// <summary>
    /// Checks to see if a vertex is at a corner. Ends are not considered corners.
    /// </summary>
    /// <param name="aSegment">A list of vertices to pick from.</param>
    /// <param name="i">Index of the vertex we're checking.</param>
    /// <returns>Is it a corner? Ends are not corners.</returns>
    public static bool                     IsSplit      (List<Vector2> aSegment, int i, List<Ferr2DT_TerrainDirection> aOverrides = null)
    {
        if (i == 0 || i == aSegment.Count - 1) return false;
        if (aOverrides!= null && aOverrides.Count < aSegment.Count) aOverrides = null;

        Ferr2DT_TerrainDirection dir1 = aOverrides == null ? 
            GetDirection(aSegment[i], aSegment[i - 1]) :
            aOverrides[i-1] == Ferr2DT_TerrainDirection.None ? 
                GetDirection(aSegment[i], aSegment[i - 1]) : 
                aOverrides[i-1];

        Ferr2DT_TerrainDirection dir2 = aOverrides == null ? 
            GetDirection(aSegment[i + 1], aSegment[i]) :
            aOverrides[i] == Ferr2DT_TerrainDirection.None ? 
                GetDirection(aSegment[i + 1], aSegment[i]) : 
                aOverrides[i];

        return dir1 != dir2;
    }
    /// <summary>
    /// Splits a path up based on corners. Corner verts are included in both segments when split.
    /// </summary>
    /// <param name="aPath">The list of path points to split.</param>
    /// <returns>An array of path segments.</returns>
    public static List<List<Vector2>>      GetSegments  (List<Vector2> aPath, out List<Ferr2DT_TerrainDirection> aSegDirections, List<Ferr2DT_TerrainDirection> aOverrides = null, bool aInvert = false, bool aClosed = false)
    {
        List<List<Vector2>> segments    = new List<List<Vector2>>();
        List<Vector2>       currSegment = new List<Vector2>();
        aSegDirections = new List<Ferr2DT_TerrainDirection>();
        int startIndex = 0;

        for (int i = 0; i < aPath.Count; i++)
        {
            currSegment.Add(aPath[i]);
            if (IsSplit(aPath, i, aOverrides))
            {
                segments.Add(currSegment);
                aSegDirections.Add(GetDirection(aPath, startIndex, aInvert, aClosed, aOverrides));

                currSegment = new List<Vector2>();
                currSegment.Add(aPath[i]);
                startIndex = i;
            }
        }
        segments.Add(currSegment);
        aSegDirections.Add(GetDirection(aPath, startIndex, aInvert, aClosed, aOverrides));
        return segments;
    }
    /// <summary>
    /// Smooths a segment of path points.
    /// </summary>
    /// <param name="aSegment">The collection of points to smooth out.</param>
    /// <param name="aSplitDistance">How far should each smooth split be?</param>
    /// <param name="aClosed">Should we close the segment?</param>
    /// <returns>A new list of vertices.</returns>
    public static List<Vector2>            SmoothSegment(List<Vector2> aSegment, float aSplitDistance, bool aClosed)
    {
        List<Vector2> result = new List<Vector2>(aSegment);
        int           curr   = 0;
        int           count  = aClosed ? aSegment.Count : aSegment.Count - 1;
        for (int i = 0; i < count; i++)
        {
            int next   = i == count - 1 ? aClosed ? 0 : aSegment.Count-1 : i+1;
            int splits = (int)(Vector2.Distance(aSegment[i], aSegment[next]) / aSplitDistance);
            for (int t = 0; t < splits; t++)
            {
                float percentage = (float)(t + 1) / (splits + 1);
                result.Insert(curr + 1, HermiteGetPt(aSegment, i, percentage, aClosed));
                curr += 1;
            }
            curr += 1;
        }
        return result;
    }
    /// <summary>
    /// This method will close a list of split segments, merging and adding points to the end chunks.
    /// </summary>
    /// <param name="aSegmentList">List of split segments that make up the path.</param>
    /// <param name="aCorners">If there are corners or not.</param>
    /// <returns>A closed loop of segments.</returns>
    public static bool                     CloseEnds(ref List<List<Vector2>> aSegmentList, ref List<Ferr2DT_TerrainDirection> aSegmentDirections, bool aCorners, bool aInverted)
    {
        Vector2 start     = aSegmentList[0][0];
        Vector2 startNext = aSegmentList[0][1];

        Vector2 end     = aSegmentList[aSegmentList.Count - 1][aSegmentList[aSegmentList.Count - 1].Count - 1];
        Vector2 endPrev = aSegmentList[aSegmentList.Count - 1][aSegmentList[aSegmentList.Count - 1].Count - 2];

        if (aCorners == false) {
            aSegmentList[0].Add(start);
            return true;
        }

        bool endCorner   = Ferr2D_Path.GetDirection(endPrev, end  ) != Ferr2D_Path.GetDirection(end,   start    );
        bool startCorner = Ferr2D_Path.GetDirection(end,     start) != Ferr2D_Path.GetDirection(start, startNext);

        if (endCorner && startCorner) {
            List<Vector2> lastSeg = new List<Vector2>();
            lastSeg.Add(end  );
            lastSeg.Add(start);

            aSegmentList.Add(lastSeg);

            Ferr2DT_TerrainDirection dir = GetDirection(start, end);
            if (aInverted && dir == Ferr2DT_TerrainDirection.Top   ) dir = Ferr2DT_TerrainDirection.Bottom;
            if (aInverted && dir == Ferr2DT_TerrainDirection.Bottom) dir = Ferr2DT_TerrainDirection.Top;
            if (aInverted && dir == Ferr2DT_TerrainDirection.Right ) dir = Ferr2DT_TerrainDirection.Left;
            if (aInverted && dir == Ferr2DT_TerrainDirection.Left  ) dir = Ferr2DT_TerrainDirection.Right;
            
            aSegmentDirections.Add(dir);
        }
        else if (endCorner && !startCorner) {
            aSegmentList[0].Insert(0, end);
        }
        else if (!endCorner && startCorner) {
            aSegmentList[aSegmentList.Count - 1].Add(start);
        }
        else {
            aSegmentList[0].InsertRange(0, aSegmentList[aSegmentList.Count - 1]);
            aSegmentList      .RemoveAt(aSegmentList      .Count - 1);
            aSegmentDirections.RemoveAt(aSegmentDirections.Count - 1);
        }
        return true;
    }

    /// <summary>
    /// A utility function! Gets the closest point on a line, clamped to the line segment provided.
    /// </summary>
    /// <param name="aStart">Start of the line segment.</param>
    /// <param name="aEnd">End of the line segment.</param>
    /// <param name="aPoint">The point to compare distance to.</param>
    /// <param name="aClamp">Should we clamp at the ends of the segment, or treat it as an infinite line?</param>
    /// <returns>The closest point =D</returns>
    public static Vector2 GetClosetPointOnLine(Vector2 aStart, Vector2 aEnd, Vector2 aPoint, bool aClamp)
    {
        Vector2 AP = aPoint - aStart;
        Vector2 AB = aEnd - aStart;
        float ab2 = AB.x*AB.x + AB.y*AB.y;
        float ap_ab = AP.x*AB.x + AP.y*AB.y;
        float t = ap_ab / ab2;
        if (aClamp)
        {
             if (t < 0.0f) t = 0.0f;
             else if (t > 1.0f) t = 1.0f;
        }
        Vector2 Closest = aStart + AB * t;
        return Closest;
    }

	public static Rect GetBounds(List<Vector2> aFrom) {
		float l = float.MaxValue;
		float r = float.MinValue;
		float t = float.MinValue;
		float b = float.MaxValue;

		for (int i = 0; i < aFrom.Count; i++) {
			if (aFrom[i].x > r) r = aFrom[i].x;
			if (aFrom[i].x < l) l = aFrom[i].x;
			if (aFrom[i].y < b) b = aFrom[i].y;
			if (aFrom[i].y > t) t = aFrom[i].y;
		}

		return new Rect(l, t, r-l, b-t);
	}
    #endregion
}
