using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplayer : MonoBehaviour
{
    public RectTransform healthBar;
    public RectTransform backGround;





    private void Start()
    {
        backGround.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,GameMaster.game.maxHealth);
    }

    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(GameMaster.game.health, 0, GameMaster.game.maxHealth));
    }
}
