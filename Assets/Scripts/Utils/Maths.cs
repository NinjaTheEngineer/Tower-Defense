using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NinjaTools {

    public class Maths : NinjaMonoBehaviour {
        public static Vector3 Clamp(Vector3 value, Vector3 minValue, Vector3 maxValue) {
            if(minValue.x > maxValue.x) {
                float tempX = minValue.x;
                minValue.x = maxValue.x;
                maxValue.x = tempX;
            } 
            if(minValue.y > maxValue.y) {
                float tempY = minValue.y;
                minValue.y = maxValue.y;
                maxValue.y = tempY;
            }
            if(minValue.z > maxValue.z) {
                float tempZ = minValue.z;
                minValue.z = maxValue.z;
                maxValue.z = tempZ;
            }
            float clampX = minValue.x==maxValue.x?value.x:Mathf.Clamp(value.x, minValue.x, maxValue.x);
            float clampY = minValue.y==maxValue.y?value.y:Mathf.Clamp(value.y, minValue.y, maxValue.y);
            float clampZ = minValue.z==maxValue.z?value.z:Mathf.Clamp(value.z, minValue.z, maxValue.z);
            Vector3 newValue = new Vector3(clampX, clampY, clampZ);
            return newValue;
        }
    }

}
