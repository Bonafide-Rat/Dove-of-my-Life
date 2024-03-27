using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Tweener : MonoBehaviour
{
    //private Tween activeTween;

    private List<Tween> activeTweens = new List<Tween>();
    private List<Tween> completedTweens = new List<Tween>();

    // Update is called once per frame
    void Update()
    {
        foreach (Tween activeTween in activeTweens)
        {
            // If the dist between the current pos and end pos is > 0.1f;
            if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.1f)
            {
                // Calculate the time elapsed since start of each tween:
                float elapsedTime = Time.time - activeTween.StartTime;

                // Calculate the standard time value for each tween using cubic interpolation:
                float timeFraction = EaseInCubic(elapsedTime / activeTween.Duration);

                // Lerp to new EndPos with time calculated;
                Vector3 newPos = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, timeFraction);

                // Set new position.
                activeTween.Target.position = newPos;

                // If tween has completed:
                if (timeFraction >= activeTween.Duration)
                {
                    //completedTweens.Add(activeTween);
                    //Debug.Log("Tween completed. Duration was: " + activeTween.Duration);
                }
            }
            else // if distance is <= 0.1f;
            {
                activeTween.Target.position = activeTween.EndPos;
                //Debug.Log("Distance was less than 0.1f");
                //activeTween = null;
            }
        }

        foreach (Tween completedTween in completedTweens)
        {
            activeTweens.Remove(completedTween);
            //Debug.Log("Tween removed.");
        }
        completedTweens.Clear(); // Reset/clear the list after removing completed tweens

    }

    /* public void OldAddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    // {
        // activeTween = new Tween (targetObject, startPos, endPos, Time.time, duration);
     } */

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        // If the tween already exists, return false.
        if (TweenExists(targetObject))
        {
            return false;
        }
        // Else, add the new tween to the list and return true.
        else
        {
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration));
            return true;
        }
    }

    private float EaseInCubic(float t)
    {
        return (t * t * t);
    }

    public bool TweenExists(Transform target)
    {
        // Loop over each tween in the list
        foreach (Tween t in activeTweens)
        {
            // If target already exists in the list, return true.
            if (t.Target == target)
            {
                return true;
            }
        }
        // If no matches found, return false.
        return false;
    }
}

