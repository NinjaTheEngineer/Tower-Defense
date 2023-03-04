using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    public static string logf(this object o) => o == null ? "NULL" : o.ToString();
}
