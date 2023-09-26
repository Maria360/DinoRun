using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using TMPro.EditorUtilities;
public class ForgetPassword : MonoBehaviour
{
    //Firebase.Auth.FirebaseAuth auth;
    //Firebase.Auth.FirebaseUser user;

    [SerializeField] TMP_InputField resetpasswordtxt;
    [SerializeField] TMP_InputField InputEmail;
    [SerializeField] TMP_InputField InputPassword;
    [SerializeField] GameObject panelRest;
    [SerializeField] TextMeshProUGUI status;


    void Start()
    {
        panelRest.SetActive(false);
        //Firebase.Auth.FirebaseUser user = auth.CurrentUser;
    }
    public void ShowPanelReset()
    {

        panelRest.SetActive(true);
    }
    void ReauthenticateUser()
    {
        var auth = FirebaseAuth.DefaultInstance;
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;


        Firebase.Auth.Credential credential =
            Firebase.Auth.EmailAuthProvider.GetCredential(InputEmail.text.ToString(), InputPassword.text.ToString());

        if (user != null)
        {
            user.ReauthenticateAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("ReauthenticateAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("ReauthenticateAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User reauthenticated successfully.");
            });
        }
    }
    public void UpdatePasswordSummit()
    {
        ReauthenticateUser();
        var auth = FirebaseAuth.DefaultInstance;
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        string newPassword = resetpasswordtxt.text.ToString();
        if (user != null)
        {
            user.UpdatePasswordAsync(newPassword).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdatePasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdatePasswordAsync encountered an error: " + task.Exception);
                    return;
                }
                status.text = "Password updated successfully";
                Debug.Log("Password updated successfully.");
                SceneManager.LoadScene("Home");
            });
        }
        else if (user == null)
        {
            Debug.Log("Ingrese Usuario");
        }

    }
}
