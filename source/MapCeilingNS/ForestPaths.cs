using UnityEngine;

namespace MapCeilingNS
{
	public class ForestPaths : MonoBehaviour
	{
		public static GameObject _line;

		public static GameObject _curve;

		private Color _color = Color.blue;

		private static float _heightsize = 0.5f;

		private static float _yAxis1 = 0f;

		private static float _yAxis2 = 15f;

		private static float _yAxis3 = 30f;

		private static float _yAxis4 = 45f;

		private static float _yAxis5 = 60f;

		private static float _yAxis6 = 75f;

		private static float _stripeWidth = 3f;

		private static float _groundWidth = 8f;

		private static float _groundheightpos = 1f;

		private static float _arrowlength = 45f;

		private static float _arrowWidth = 6f;

		private static float _treeHeight = 1f;

		private static float _treeHeightPos = 25f;

		public static void CreateForestPaths()
		{
			StripesWall(-63f, -488f, 55f, Quaternion.identity);
			StripesWall(-70f, -70f, 330f, Quaternion.identity);
			StripesWall(-90f, 165f, 160f, Quaternion.identity);
			StripesWall(-100f, 415f, 80f, Quaternion.identity);
			StripesWall(-66f, -240f, 15f, new Quaternion(0.9f, 0f, 0.31f, 0f));
			StripesWall(-80f, 100f, 25f, new Quaternion(0.9f, 0f, 0.6f, 0f));
			StripesWall(-95f, 250f, 15f, new Quaternion(0.9f, 0f, 0.6f, 0f));
			TreeTube(-300f, -380f, 1.1f);
			TreeTube(-284f, -265f, 1.2f);
			TreeTube(-67f, -458f, 1.2f);
			TreeTube(-288f, 427f, 0.9f);
			TreeTube(28f, 434f, 1.2f);
			TreeTube(438.8f, -104f, 1f);
			TreeTubeYellow(463.4f, -31.7f, 1f);
			TreeTubeYellow(59f, -317.6f, 1.4f);
			TreeTubeRed(388.6f, -260.3f, 1f);
			TreeTubeRed(294.5f, -342.5f, 1.1f);
			GroundStripe(-200f, 0f, 800f, new Quaternion(0.08f, 0f, 0.9f, 0f), Color.yellow);
			GroundStripeArrowYellow(-280f, 500f, new Quaternion(0f, 0f, 0.7f, 0f));
			GroundStripe(-220f, 400f, 260f, new Quaternion(1f, 0f, 0.23f, 0f), Color.yellow);
			GroundStripeArrowYellow(-262f, 510f, new Quaternion(0.6f, 0f, 0.3f, 0f));
			GroundStripe(-324f, 0f, 800f, Quaternion.identity, Color.green);
			GroundStripe(-425f, 0f, 800f, Quaternion.identity, Color.green);
			GroundStripe(-405f, 0f, 400f, new Quaternion(0.99f, 0f, 0.05f, 0f), Color.green);
			GroundStripe(-595f, 0f, 440f, Quaternion.identity, Color.green);
			GroundStripe(-520f, 0f, 440f, Quaternion.identity, Color.green);
			GroundStripe(-176f, -160f, 700f, new Quaternion(0.99f, 0f, 0.09f, 0f), Color.cyan);
			StripesWall(-168f, 30f, 175f, Quaternion.identity);
			StripesWall(-157f, -88f, 60f, new Quaternion(0.9f, 0f, 0.14f, 0f));
			StripesWall(-145f, -160f, 80f, new Quaternion(0.9f, 0f, 0.12f, 0f));
			StripesWall(-165f, -295f, 50f, new Quaternion(0.9f, 0f, 0.12f, 0f));
			StripesWall(-240f, 60f, 55f, Quaternion.identity);
			StripesWall(-290f, 168f, 110f, new Quaternion(0.1f, 0f, 0.9f, 0f));
			StripesWallNoBarrier(-174f, 154f, 64f, new Quaternion(0.2f, 0f, -0.9f, 0f));
			StripesWallNoBarrier(-153f, -232f, 70f, new Quaternion(0.25f, 0f, 0.9f, 0f));
			StripesWall(-170f, 430f, 160f, new Quaternion(0.1f, 0f, 0.9f, 0f));
			StripesWall(-390f, 180f, 100f, new Quaternion(0.99f, 0f, 0.09f, 0f));
			StripesWall(-448f, 210f, 90f, new Quaternion(0f, 0f, 0f, 0f));
			StripesWall(-555f, 95f, 70f, new Quaternion(0.1f, 0f, 0f, 0f));
			StripesWall(-553f, -5f, 135f, Quaternion.identity);
			StripesWall(-545f, -125f, 105f, new Quaternion(1f, 0f, 0.05f, 0f));
			StripesWall(-471f, -300f, 60f, new Quaternion(1f, 0f, 0.07f, 0f));
			StripesWall(-478f, -235f, 75f, new Quaternion(0.05f, 0f, -0.99f, 0f));
			StripesWall(-475f, -155f, 88f, new Quaternion(-0.9f, 0f, 0.05f, 0f));
			GroundStripesCurve(130f, 600f, 11f, new Quaternion(1f, 0f, -1f, 0f));
			GroundStripesCurve(120f, 490f, 13f, new Quaternion(1f, 0f, -1f, 0f));
			GroundStripesCurve(560f, -90f, 20f, new Quaternion(0f, 0f, 1f, 0f));
			StripesWallNoBarrier(60f, 110f, 90f, new Quaternion(0.99f, 0f, 0.25f, 0f));
			GroundStripe(260f, 180f, 800f, new Quaternion(0.5f, 0f, 0.174f, 0f), Color.green);
			GroundStripeArrowYellow(495f, 74f, new Quaternion(0.5f, 0f, 0.76f, 0f));
			GroundStripe(270f, -150f, 660f, new Quaternion(-0.5f, 0f, 0.215f, 0f), Color.yellow);
			GroundStripeArrowYellow(506f, 63f, new Quaternion(0.1f, 0f, 0.42f, 0f));
			GroundStripe(142f, -180f, 730f, new Quaternion(0.08f, 0f, 0.9f, 0f), Color.yellow);
			StripesWallNoBarrier(210f, 175f, 40f, new Quaternion(0.1f, 0f, 0.15f, 0f));
			GroundStripe(183f, -160f, 640f, new Quaternion(0.5f, 0f, 0.08f, 0f), Color.red);
			GroundStripe(55f, 110f, 90f, new Quaternion(0.5f, 0f, -0.17f, 0f), Color.red);
			GroundStripeBig(350f, 30f, 800f, new Quaternion(0.5f, 0f, 0.07f, 0f), Color.red);
			GroundStripe(270f, 0f, 800f, new Quaternion(0.5f, 0f, 0.08f, 0f), Color.red);
		}

		private static void TreeTube(float xAxis, float zAxis, float treeWidth)
		{
			GameObject obj = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("checkpointtube"), Vector3.zero, Quaternion.identity);
			obj.transform.position = new Vector3(xAxis, _treeHeightPos, zAxis);
			obj.transform.localScale = new Vector3(treeWidth, _treeHeight, treeWidth);
			obj.transform.rotation = Quaternion.identity;
		}

		private static void TreeTubeRed(float xAxis, float zAxis, float treeWidth)
		{
			GameObject obj = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("killtube"), Vector3.zero, Quaternion.identity);
			obj.transform.position = new Vector3(xAxis, _treeHeightPos, zAxis);
			obj.transform.localScale = new Vector3(treeWidth, _treeHeight, treeWidth);
			obj.transform.rotation = Quaternion.identity;
		}

		private static void TreeTubeYellow(float xAxis, float zAxis, float treeWidth)
		{
			GameObject obj = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("endtube"), Vector3.zero, Quaternion.identity);
			obj.transform.position = new Vector3(xAxis, _treeHeightPos, zAxis);
			obj.transform.localScale = new Vector3(treeWidth, _treeHeight, treeWidth);
			obj.transform.rotation = Quaternion.identity;
		}

		private static void GroundStripe(float xAxis, float zAxis, float length, Quaternion rotation, Color color)
		{
			GameObject obj = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("endcuboid"), Vector3.zero, Quaternion.identity);
			obj.transform.position = new Vector3(xAxis, _groundheightpos, zAxis);
			obj.transform.localScale = new Vector3(_groundWidth, _heightsize, length);
			obj.transform.rotation = rotation;
			obj.renderer.material.color = color;
		}

		private static void GroundStripeBig(float xAxis, float zAxis, float length, Quaternion rotation, Color color)
		{
			GameObject obj = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("endcuboid"), Vector3.zero, Quaternion.identity);
			obj.transform.position = new Vector3(xAxis, _groundheightpos, zAxis);
			obj.transform.localScale = new Vector3(20f, _heightsize, length);
			obj.transform.rotation = rotation;
			obj.renderer.material.color = color;
		}

		private static void GroundStripesCurve(float xAxis, float zAxis, float length, Quaternion rotation)
		{
			GameObject obj = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("checkpointArc180"), Vector3.zero, Quaternion.identity);
			obj.transform.position = new Vector3(xAxis, 0f, zAxis);
			obj.transform.localScale = new Vector3(_stripeWidth, 1f, length);
			obj.transform.rotation = rotation;
		}

		private static void GroundStripeArrowYellow(float xAxis, float zAxis, Quaternion rotation)
		{
			GameObject obj = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("endcuboid"), Vector3.zero, Quaternion.identity);
			obj.transform.position = new Vector3(xAxis, _groundheightpos, zAxis);
			obj.transform.localScale = new Vector3(_arrowWidth, _heightsize, _arrowlength);
			obj.transform.rotation = rotation;
		}

		private static void GroundStripeArrowGreen(float xAxis, float zAxis, Quaternion rotation)
		{
			GameObject obj = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("checkpointcuboid"), Vector3.zero, Quaternion.identity);
			obj.transform.position = new Vector3(xAxis, _groundheightpos, zAxis);
			obj.transform.localScale = new Vector3(_arrowWidth, _heightsize, _arrowlength);
			obj.transform.rotation = rotation;
		}

		private static void StripesWall(float xAxis, float zAxis, float length, Quaternion rotation)
		{
			GameObject obj = new GameObject();
			obj.AddComponent<ForestPaths>();
			obj.transform.position = new Vector3(xAxis, _yAxis1, zAxis);
			obj.transform.localScale = new Vector3(_stripeWidth, _heightsize, length);
			obj.transform.rotation = rotation;
			GameObject obj2 = new GameObject();
			obj2.AddComponent<ForestPaths>();
			obj2.transform.position = new Vector3(xAxis, _yAxis2, zAxis);
			obj2.transform.localScale = new Vector3(_stripeWidth, _heightsize, length);
			obj2.transform.rotation = rotation;
			GameObject obj3 = new GameObject();
			obj3.AddComponent<ForestPaths>();
			obj3.transform.position = new Vector3(xAxis, _yAxis3, zAxis);
			obj3.transform.localScale = new Vector3(_stripeWidth, _heightsize, length);
			obj3.transform.rotation = rotation;
			GameObject obj4 = new GameObject();
			obj4.AddComponent<ForestPaths>();
			obj4.transform.position = new Vector3(xAxis, _yAxis4, zAxis);
			obj4.transform.localScale = new Vector3(_stripeWidth, _heightsize, length);
			obj4.transform.rotation = rotation;
			GameObject obj5 = new GameObject();
			obj5.AddComponent<ForestPaths>();
			obj5.transform.position = new Vector3(xAxis, _yAxis5, zAxis);
			obj5.transform.localScale = new Vector3(_stripeWidth, _heightsize, length);
			obj5.transform.rotation = rotation;
			GameObject obj6 = new GameObject();
			obj6.AddComponent<ForestPaths>();
			obj6.transform.position = new Vector3(xAxis, _yAxis6, zAxis);
			obj6.transform.localScale = new Vector3(_stripeWidth, _heightsize, length);
			obj6.transform.rotation = rotation;
			GameObject obj7 = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("barrier"), Vector3.zero, Quaternion.identity);
			obj7.transform.position = new Vector3(xAxis, 45f, zAxis);
			obj7.transform.localScale = new Vector3(_stripeWidth, 90f, length);
			obj7.transform.rotation = rotation;
		}

		private static void StripesWallNoBarrier(float xAxis, float zAxis, float length, Quaternion rotation)
		{
			GameObject obj = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("killcuboid"), Vector3.zero, Quaternion.identity);
			obj.transform.position = new Vector3(xAxis, _yAxis1, zAxis);
			obj.transform.localScale = new Vector3(_stripeWidth, _heightsize, length);
			obj.transform.rotation = rotation;
			obj.renderer.material.color = Color.blue;
			GameObject obj2 = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("killcuboid"), Vector3.zero, Quaternion.identity);
			obj2.transform.position = new Vector3(xAxis, _yAxis2, zAxis);
			obj2.transform.localScale = new Vector3(_stripeWidth, _heightsize, length);
			obj2.transform.rotation = rotation;
			obj2.renderer.material.color = Color.blue;
			GameObject obj3 = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("killcuboid"), Vector3.zero, Quaternion.identity);
			obj3.transform.position = new Vector3(xAxis, _yAxis3, zAxis);
			obj3.transform.localScale = new Vector3(_stripeWidth, _heightsize, length);
			obj3.transform.rotation = rotation;
			obj3.renderer.material.color = Color.blue;
			GameObject obj4 = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("killcuboid"), Vector3.zero, Quaternion.identity);
			obj4.transform.position = new Vector3(xAxis, _yAxis4, zAxis);
			obj4.transform.localScale = new Vector3(_stripeWidth, _heightsize, length);
			obj4.transform.rotation = rotation;
			obj4.renderer.material.color = Color.blue;
			GameObject obj5 = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("killcuboid"), Vector3.zero, Quaternion.identity);
			obj5.transform.position = new Vector3(xAxis, _yAxis5, zAxis);
			obj5.transform.localScale = new Vector3(_stripeWidth, _heightsize, length);
			obj5.transform.rotation = rotation;
			obj5.renderer.material.color = Color.blue;
			GameObject obj6 = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load("killcuboid"), Vector3.zero, Quaternion.identity);
			obj6.transform.position = new Vector3(xAxis, _yAxis6, zAxis);
			obj6.transform.localScale = new Vector3(_stripeWidth, _heightsize, length);
			obj6.transform.rotation = rotation;
			obj6.renderer.material.color = Color.blue;
		}

		private GameObject CreateGO(string asset)
		{
			GameObject obj = (GameObject)Object.Instantiate(FengGameManagerMKII.RCassets.Load(asset), Vector3.zero, Quaternion.identity);
			obj.transform.position = base.transform.position;
			obj.transform.localScale = base.transform.localScale;
			obj.transform.rotation = base.transform.rotation;
			return obj;
		}

		private void Start()
		{
			_line = CreateGO("killcuboid");
		}
	}
}
