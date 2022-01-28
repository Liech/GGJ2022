using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetImpulse : MonoBehaviour
{
  public float cooldownDuration   = 2;
  public KeyCode activationKey = KeyCode.E;

  public float range              = 5;
  public float impulseFactor      = 1.5f;
  public float impulseAdd         = 0.5f;
  public float reflectPercentage  = 0.5f;
  public float directPercentage   = 0.5f;

  private bool ready = true;
  private Rigidbody2D body;

  void Start() {
  }

  private void Update() {
    if(Input.GetKeyDown(KeyCode.E) && ready) {
      StartCoroutine(cooldown());
      impulse();
    }
  }

  void impulse() {
    foreach(var obj in GameObject.FindObjectsOfType<AffectedFromMagnetic>()) {
      var body=obj.GetComponent<Rigidbody2D>();
      var diff = obj.transform.position - transform.position;
      if(diff.magnitude>range)
        continue;

      float magneticDir = (GetComponent<Magnetic>().MagnetType==obj.GetComponent<Magnetic>().MagnetType)?1:-1;
      Vector2 reflect = Vector2.Reflect(obj.GetComponent<Rigidbody2D>().velocity,-diff.normalized);
      Vector2 dir     = diff.normalized;
      float speed = body.velocity.magnitude * impulseFactor + impulseAdd;
      var newVel = reflect * reflectPercentage * speed + directPercentage * speed * dir;
      obj.GetComponent<Rigidbody2D>().velocity=newVel*magneticDir;
    }

  }

  IEnumerator cooldown() {
    ready=false;
    yield return new WaitForSeconds(cooldownDuration);
    ready=true;
  }

  void OnDrawGizmos() {
    if (ready)
      Gizmos.DrawWireSphere(transform.position, range);
  }
}