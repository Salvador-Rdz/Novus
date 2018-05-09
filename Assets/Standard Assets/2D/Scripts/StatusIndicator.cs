//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

	[SerializeField] //Esto se pone para que aparezcan en el inspector a pesar de ser variables privadas
	private RectTransform healthBarRect;
	[SerializeField]
	private Text healthText;
	
	void Start () {
		
		if(healthBarRect == null) {
			Debug.LogError("STATUS INDICATOR: No hay referencia a un objeto de 'healthBar'!!!!!!");
		}
		if(healthText == null) {
			Debug.LogError("STATUS INDICATOR: No hay referencia a un objeto de 'healthText'!!!!!!");
		}
	}
	
	public void SetHealth (int _cur, int _max) {
		
		float _value = (float) _cur / _max; //Para calcula el tamaño de la barra de vida en base a la "salud" actual comparado con el maximo posible
		
		healthBarRect.localScale = new Vector3 (_value, healthBarRect.localScale.y, healthBarRect.localScale.z); //Esto ira cambiando el tamaño de la barrita de vida
		healthText.text = _cur + "/" + _max; //Mostrata la vida que le queda al enemigo en el siguiente formato "100/100" o "35/100" dependiendo el daño que reciba
		
	}
}
