

using UnityEngine;
using System.Xml;
using System.Collections;


public partial class PUSprite : PUSpriteBase {

	public SpriteRenderer spriteRenderer = null;
	public Sprite[] allSprites = null;

	public override void gaxb_load(XmlReader reader, object _parent, Hashtable args)
	{
		allSprites = Resources.LoadAll<Sprite>(resourcePath);

		gameObject = new GameObject("<Sprite />");
		gameObject.AddComponent<SpriteRenderer>();

		if (titleExists) {
			gameObject.name = title;
		}

		base.gaxb_load(reader, _parent, args);

		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		gameObject.GetComponent<SpriteRenderer>().sprite = allSprites[0];

		if (scaleExists) {
			gameObject.transform.localScale = new Vector3 (scale, scale, 1);
		}
		if (positionExists) {
			if (_parent is PUGameObject) {
				PUGameObject parentEntity = (PUGameObject)_parent;
				gameObject.transform.localPosition = new Vector3 (position.x, parentEntity.bounds.h-position.y, position.z);
			}
		}

		gameObject.renderer.material.renderQueue = scope().getRenderQueue()+renderQueueOffset;
	}

}
