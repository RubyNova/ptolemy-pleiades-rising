using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

public float vertSpeed = 5f;
public float horzSpeed = 0f;
float leftPoint, rightPoint, topPoint, bottomPoint;
float contactX, contactY;
CircleCollider2D cc;
BoxCollider2D bc;
int horz, vert;

void Start()
{
    if (vertSpeed > 0f) vert = 1;
    if (vertSpeed < 0f) vert = -1;
    if (horzSpeed > 0f) horz = 1;
    if (horzSpeed < 0f) horz = -1;
}

    void Update()
    {
	    transform.Translate(new Vector3(horzSpeed,vertSpeed,0) * Time.deltaTime);
        if (cc = GetComponent<CircleCollider2D>())
        {
            leftPoint = (cc.radius * transform.localScale.x * -1f) *.97f;
            rightPoint = (cc.radius * transform.localScale.x) *.97f;
            topPoint = (cc.radius * transform.localScale.y) *.97f;
            bottomPoint = (cc.radius * transform.localScale.y * -1f) *.97f;
        }
        if (bc = GetComponent<BoxCollider2D>())
        {
            leftPoint = ((bc.size.x / 2f) * transform.localScale.x * -1f) *.97f;
            rightPoint = ((bc.size.x / 2f) * transform.localScale.x) * .97f;
            topPoint = ((bc.size.y / 2f) * transform.localScale.y) * .97f;
            bottomPoint = ((bc.size.y / 2f) * transform.localScale.y * -1f) * .97f;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
	{
        contactX = coll.contacts[0].point.x - transform.position.x;
        contactY = coll.contacts[0].point.y - transform.position.y;


	    if((contactX > rightPoint && horz == 1) || (contactX < leftPoint && horz == -1)){
		    horzSpeed *= -1;
            horz*= -1;
	    }
	    if((contactY > topPoint && vert == 1) || (contactY < bottomPoint && vert == -1)){
		    vertSpeed *= -1;
            vert *= -1;
	    }
    }
}
