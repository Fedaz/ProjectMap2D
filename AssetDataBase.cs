using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class AssetDataBase :  EditorWindow {

    private Object _focusObject;
    private string _searchAssetByName;
    private string _newName;
    private List<Object> _assets = new List<Object>();

	private Texture2D _preview;
    Vector2 _scrollPosition;

    private GameObject GO;

    private Renderer rend;

	private Material[] faceMaterials = new Material[4];



    [MenuItem("AssetDataBase/CargarTextures")]
    static void CreateAssetDataBase()
    {
        ((AssetDataBase)GetWindow(typeof(AssetDataBase))).Show(); //abre la ventana del AssetDataBase
    }

    void OnGUI()
    {

        EditorGUILayout.LabelField("TEXTURA"); //texto arrastrar textura

		_focusObject = EditorGUILayout.ObjectField(_focusObject, typeof(Object), true); //creamos una varible donde va a estar el objeto

        if (_focusObject != null) //si la variable no contiene objeto entonces...
        {
            if (AssetDatabase.Contains(_focusObject)) //AssetDatabase.Contains() nos dice si el objeto es un prefab o no.
            {
                Herramientas();
            }
            else //si no, avisa que no es una textura
            {
                EditorGUILayout.HelpBox("El objeto no es una textura", MessageType.Error);
            }
        }
        else
        {
            EditorGUILayout.LabelField("----------------------------------------------------------------------------"); //texto
            BuscarTextura();
        }
    }

    private void Herramientas()
    {
        EditorGUILayout.LabelField("Ruta: " + AssetDatabase.GetAssetPath(_focusObject)); //AssetDatabase.GetAssetPath() nos da el path(Ruta) de un objeto
        EditorGUILayout.LabelField("----------------------------------------------------------------------------"); //texto

		#region Preview
		//MOSTRAMOS LA PREVIEW DE LA TEXTURA SELECCIONADA
		_preview = AssetPreview.GetAssetPreview(_focusObject);
		if (_preview != null)
		{
			Repaint();
			GUILayout.BeginHorizontal();
			GUI.DrawTexture(GUILayoutUtility.GetRect(50, 50, 50, 50), _preview, ScaleMode.ScaleToFit);
			GUILayout.Label(_focusObject.name);
			GUILayout.Label(AssetDatabase.GetAssetPath(_focusObject));
			GUILayout.EndHorizontal();
			faceMaterials[0] = matSetup( Color.white, _preview); //ACA LE DECIMOS QUE EL MATERIAL GUARDADO EN EL ARRAY VA A SER BLANCO Y CON LA TEXTURA DE _PREVIEW
			faceMaterials[0].name = "Face"; //Y VA A TENER COMO NOMBRE "Face"
		}
		else
			EditorGUILayout.LabelField("No existe ninguna preview");
		#endregion	

        #region Renombrar Textura
        EditorGUILayout.HelpBox("RENOMBRAR", MessageType.Info);

        _newName = EditorGUILayout.TextField("Nuevo Nombre", _newName);

        if(GUILayout.Button("Renombrar"))
        {
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(_focusObject), _newName); //AssetDatabase.RenameAsset() renombra el asset
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        #endregion

        #region Seleccionar Otra Textura
        EditorGUILayout.HelpBox("SELECCIONAR OTRA TEXTURA", MessageType.Info);

        if (GUILayout.Button("Seleccionar otra Textura"))
        {
            _focusObject = null;
        }
        #endregion

        #region pintar


        EditorGUILayout.LabelField("----------------------------------------------------------------------------"); //texto

        EditorGUILayout.LabelField("SELECCIONAR TILE"); //texto arrastrar textura

        EditorGUILayout.HelpBox("ARRASTRAR TILE", MessageType.Info);

        GO = (GameObject)EditorGUILayout.ObjectField("Tile: ", GO, typeof(GameObject), true); //asignamos el tile que va a ser la aplicado con una textura

        if (GUILayout.Button("Pintar Tile"))
        {
            rend = GO.GetComponent<Renderer>(); //obtenemos el renderer del Tile que queremos pintar
			rend.material = faceMaterials[0]; //igualamos el material del tile que queremos pintar al material que tenemos guardado en el array
        }      
        #endregion

    }

    private void BuscarTextura()
    {
        EditorGUILayout.LabelField("BUSCAR TEXTURA"); //texto buscar textura

        var aux = _searchAssetByName; //creamos una variable auxiliar donde va a guardar el nombre por el cual estamos buscando a la textura(Asset)

        _searchAssetByName = EditorGUILayout.TextField(aux);

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true, GUILayout.Height(150));

        if (aux != _searchAssetByName)
        {
            _assets.Clear();

            string[] allPaths = AssetDatabase.FindAssets(_searchAssetByName +" t:Texture2D"); //AssetDatabase.FindAssets busca los paths en GUID de todos los assets que coinciden con el parametro que buscamos        

            for(int i = allPaths.Length - 1; i >= 0; i--)
            {
                allPaths[i] = AssetDatabase.GUIDToAssetPath(allPaths[i]); //AssetDatabase.GUIDToAssetPath() Convierte de GUID a Path
                _assets.Add(AssetDatabase.LoadAssetAtPath(allPaths[i], typeof(Object))); //AssetDatabase.LoadAssetAtPath carga un objeto en una ubicacion
			}
        }

        #region Botones Seleccionar
        for (int j = _assets.Count - 1; j >= 0; j--)
        {
            EditorGUILayout.BeginHorizontal(); //empieza el horizontal

            EditorGUILayout.LabelField(_assets[j].ToString());

            if (GUILayout.Button("Seleccionar"))
            {
                _focusObject = _assets[j];

				//EditorGUI.DrawPreviewTexture(Rect(25,60,100,100),textureToPreview);
            }

            EditorGUILayout.EndHorizontal(); //termina el horizontal
        }
        #endregion

        EditorGUILayout.EndScrollView(); //aca termina el vertical
    }

	Material matSetup(Color col , Texture tex) //le pasamos los argumentos obtenidos al cargar la preview
	{
		Material mat = new Material(Shader.Find("Diffuse"));
		mat.SetColor ("_Color", col);
		mat.SetTexture("_MainTex", tex);
		return mat;
	}
}
