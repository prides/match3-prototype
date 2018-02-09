using System.Collections;
using UnityEngine;

public class TextValueChanger : MonoBehaviour, IValueChangeListener
{
    public TextMesh valueTextMesh;
    private int currentValue = 0;
    private int visualizeValue = 0;
    private bool isUpdating = false;

    public void OnValueChange(object sender, int value)
    {
        if (currentValue != value)
        {
            currentValue = value;
            UpdateValue();
        }
    }

    private void UpdateValue()
    {
        if (isUpdating)
        {
            return;
        }
        isUpdating = true;
        StartCoroutine(DoUpdate(0.15f));
    }

    private IEnumerator DoUpdate(float waitTime)
    {
        while (currentValue != visualizeValue)
        {
            visualizeValue += currentValue > visualizeValue ? 1 : -1;
            if (null != valueTextMesh)
            {
                valueTextMesh.text = visualizeValue.ToString();
            }
            yield return new WaitForSeconds(waitTime);
        }
        isUpdating = false;
    }
}