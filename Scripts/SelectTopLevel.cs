using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helper script for selecting top-level parent in Unity Editor Scene window
// attach to the top-most game object to select it instead of a random child component when selecting inside the Scene window.

[SelectionBase]
public class SelectTopLevel : MonoBehaviour {}
