using UnityEngine;
using UnityEditor;

namespace Hekatombe.Base.Editor
{
	public class CreateGameobjectOnKeyPress : EditorWindow {
		private string _name = "Name";
		private Transform _lastTransformSelected;

		[MenuItem("Window/Create Gameobject On Key Press")]
		public static void ShowWindow() {
			EditorWindow.GetWindow(typeof(CreateGameobjectOnKeyPress));
		}

		void OnEnable()
		{
			SceneView.onSceneGUIDelegate = SceneUpdate;
		}

		//private bool _disable
		private void SceneUpdate(SceneView scene)
		{
			Event e = Event.current;

			if (e.type == EventType.KeyDown)
			{
				//Debug.Log ("KeyCode: " + e.keyCode);
				switch(e.keyCode)
				{
				case KeyCode.C:
					CreateGameObject (GetMousePos(e.mousePosition));
					break;
				case KeyCode.D:
					DestroyNearestGameObject (GetMousePos(e.mousePosition));
					break;
				}
			}
		}

		private Vector3 GetMousePos(Vector2 mousePosition)
		{
			Ray r = Camera.current.ScreenPointToRay(new Vector3(mousePosition.x, -mousePosition.y + Camera.current.pixelHeight));
			return r.origin;
		}

		void OnGUI() {
			//Transform curr = Selection.activeTransform;
			GUILayout.Label ("Creates a GameObject in the selected Transform");
			GUILayout.Space(20);

			GUILayout.BeginHorizontal();
			GUILayout.Label(" GO Name ");
			_name = EditorGUILayout.TextField (_name);// .StringField(grid.width, GUILayout.Width(200));
			GUILayout.EndHorizontal();


			GUILayout.Space(20);
			string selectedParent = "Selected Parent: ";
			if (_lastTransformSelected == null) {
				selectedParent += "None";
			} else {
				selectedParent += _lastTransformSelected.name;
			}
			GUILayout.Label (selectedParent);
			//Button Select Parent
			if (GUILayout.Button ("Select Parent")) {
				_lastTransformSelected = Selection.activeTransform;
			}

			GUILayout.Space(20);
			GUILayout.Label ("Careful with this...");
			//Button Select Parent
			if (GUILayout.Button ("Remove ALL")) {
				ShowDialogConfirmation ();
				//RemoveAllChilds ();
			}
			//GUILayout.Label(error);
		}

		private void CreateGameObject(Vector3 mousePos) {

			if (_lastTransformSelected == null) {
				Debug.Log ("Select a Parent Transform to create the GameObject");    
				return;
			}

			GameObject go = new GameObject ();
			go.transform.position = mousePos;
			go.transform.SetParent (_lastTransformSelected);
			go.name = _name;
			go.transform.localPosition = go.transform.localPosition.CopyVectorButModifyY (0);
		}

		private void DestroyNearestGameObject(Vector3 mousePos)
		{
			Transform childToDelete;
			if (_lastTransformSelected == null) {
				return;
			}
			float distance = float.MaxValue;
			Transform childCloser = null;
			for(int i=0; i<_lastTransformSelected.childCount; i++)
			{
				float distNow = Vector3.Distance (mousePos, _lastTransformSelected.GetChild (i).position);
				if (i == 0) {
					distance = distNow;
					childCloser = _lastTransformSelected.GetChild (i);
				} else if (distNow < distance) {
					distance = distNow;
					childCloser = _lastTransformSelected.GetChild (i);
				}
			}
			if (childCloser != null) {
				DestroyImmediate (childCloser.gameObject);
			}
		}

		private void ShowDialogConfirmation()
		{
			if (EditorUtility.DisplayDialog("Confirm Remove ALL", 
				"Do you really want to delete all the CHILDS of " + _lastTransformSelected.name + "?",
				"OK",
				"Cancel"))
			{
				RemoveAllChilds();
			}
		}

		private void RemoveAllChilds()
		{
			if (_lastTransformSelected == null) {
				return;
			}
			for(int i=0; i<_lastTransformSelected.childCount; i++)
			{
				DestroyImmediate (_lastTransformSelected.GetChild(i).gameObject);
				i--;
			}
		}
	}
}