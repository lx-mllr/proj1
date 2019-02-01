using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class OnQuit : MonoBehaviour
{
    [Inject] readonly User _user;

    void OnApplicationQuit () {
        _user.SaveState();
    }
}
