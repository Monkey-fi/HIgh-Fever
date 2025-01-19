using UnityEngine;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
    public Slider sensitivitySlider; // Reference to the UI Slider
    public Text sensitivityValueText; // Optional: To display the slider value

    void Start()
    {
        // Add listener to handle slider value changes
        sensitivitySlider.onValueChanged.AddListener(OnSliderValueChanged);

        // Initialize the slider value display
        UpdateSensitivityValueText(sensitivitySlider.value);
    }

    // Callback function when the slider value changes
    void OnSliderValueChanged(float value)
    {
        UpdateSensitivityValueText(value);
    }

    // Update the value display text
    void UpdateSensitivityValueText(float value)
    {
        if (sensitivityValueText != null)
        {
            sensitivityValueText.text = $"Value: {value:F1}";
        }
    }
}
