<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs"
Inherits="welfareSystem.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <title>Login</title>
    <link
      href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
      rel="stylesheet"
    />
    <link
      href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css"
      rel="stylesheet"
    />

    <style>
      .login-wrapper {
        min-height: 85vh;
        display: flex;
        justify-content: center;
        align-items: center;
        background: #f4f7fb;
        padding: 30px;
      }

      .login-card {
        width: 420px;
        background: #fff;
        border-radius: 20px;
        padding: 40px;
        box-shadow: 0px 10px 30px rgba(0, 0, 0, 0.12);
      }

      .login-title {
        text-align: center;
        font-size: 32px;
        font-weight: bold;
        color: #1b3c59;
        margin-bottom: 10px;
      }

      .login-subtitle {
        text-align: center;
        color: gray;
        margin-bottom: 30px;
      }

      .form-control {
        height: 50px;
        border-radius: 12px;
      }

      .btn-login {
        width: 100%;
        height: 50px;
        border-radius: 12px;
        border: none;
        background: #1b3c59;
        color: white;
        font-size: 17px;
        font-weight: 600;
        transition: 0.3s;
      }

      .btn-login:hover {
        background: #12293d;
      }

      .footer-link {
        text-align: center;
        margin-top: 20px;
      }

      .footer-link a {
        text-decoration: none;
        font-weight: 600;
        color: #1b3c59;
      }
    </style>
  </head>
  <body>
    <form id="form1" runat="server">
      <div class="login-wrapper">
        <div class="login-card">
          <div class="login-title">HMIS Enhancement Portal</div>

          <div class="login-subtitle">Sign In To Continue</div>

          <div class="mb-3">
            <label> Username </label>

            <asp:TextBox
              ID="txtUsername"
              runat="server"
              CssClass="form-control"
              placeholder="Enter Username"
            >
            </asp:TextBox>
          </div>

          <div class="mb-4">
            <label> Password </label>

            <div class="input-group">
              <asp:TextBox
                ID="txtPassword"
                runat="server"
                TextMode="Password"
                CssClass="form-control"
                placeholder="Enter Password"
              >
              </asp:TextBox>
              <button
                type="button"
                class="btn btn-outline-secondary"
                onclick="togglePassword()"
                id="togglePasswordBtn">
                <i class="fas fa-eye" id="eyeIcon"></i>
              </button>
            </div>
          </div>

          <asp:Button
            ID="btnLogin"
            runat="server"
            Text="LOGIN"
            CssClass="btn-login"
            OnClick="btnLogin_Click"
          />

          <div class="footer-link">
            Don't have an account?
            <a href="#"> Contact HMC-IT Team </a>
          </div>
        </div>
      </div>
    </form>

    <script>
      function togglePassword() {
        var passwordTextBox = document.getElementById(
          "<%= txtPassword.ClientID %>",
        );
        var eyeIcon = document.getElementById("eyeIcon");

        if (passwordTextBox.type === "password") {
          passwordTextBox.type = "text";
          eyeIcon.classList.remove("fa-eye");
          eyeIcon.classList.add("fa-eye-slash");
        } else {
          passwordTextBox.type = "password";
          eyeIcon.classList.remove("fa-eye-slash");
          eyeIcon.classList.add("fa-eye");
        }
      }
    </script>
  </body>
</html>
