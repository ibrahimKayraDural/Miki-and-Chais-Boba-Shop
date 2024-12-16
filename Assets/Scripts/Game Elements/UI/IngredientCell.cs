using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientCell : MonoBehaviour
{
    [SerializeField] Image _ItemIcon;

    public void Initialize(ItemData data)
    {
        if (data == null || data.UISprite == null || data.UseSpritePrefab)
        {
            Destroy(gameObject);
            return;
        }

        _ItemIcon.sprite = data.UISprite;
    }
}
