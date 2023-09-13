using System.Text.RegularExpressions;
using UnityEngine;

namespace Menu
{
    [RequireComponent(typeof(TextMesh))]
    public class MockData : MonoBehaviour
    {
        private void Start()
        {
            var match = Regex.Match(gameObject.name, @"\d+");
            GetComponent<TextMesh>().text = match.Value;
        }
    }
}