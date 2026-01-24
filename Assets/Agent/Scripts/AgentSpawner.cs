using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AgentSpawner : MonoBehaviour
{
    [SerializeField] AIAgent[] agents;
    [SerializeField] LayerMask layerMask = Physics.AllLayers;
    [SerializeField] TextMeshProUGUI infoText;

    Camera activeCamera;
    int agentIndex = 0;

    void Start()
    {
        activeCamera = Camera.main;
        if (infoText != null)
        {
            infoText.text = $"Selected Agent: {agents[agentIndex].name}";

        }
    }

    void Update()
    {
        // use key to select agent type
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            
            agentIndex = ++agentIndex % agents.Length;
            if (infoText != null)
            {
                infoText.text = $"Selected Agent: {agents[agentIndex].name}";

            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame ||
           (Mouse.current.leftButton.IsPressed() && Keyboard.current.leftCtrlKey.isPressed))
        {
            Ray ray = activeCamera.ScreenPointToRay(Mouse.current.position.value);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100.0f, layerMask))
            {
                Instantiate(agents[agentIndex], hitInfo.point, Quaternion.Euler(0, Random.Range(0, 360), 0));
            }
        }
    }
}
