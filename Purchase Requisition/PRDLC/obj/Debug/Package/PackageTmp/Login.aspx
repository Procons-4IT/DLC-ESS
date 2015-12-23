<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="PRDLC.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <title>
      Login Page
    </title>
    <link rel="stylesheet" type="text/css" href="Styles/style.css" />
  
</head>
<body>

<span  class="button" id="toggle-login" style="width:210px;">Purchase Requisition (DLC)</span>

<div id="login">
  <div id="triangle"></div>
  <h1>Log in</h1>
  <form id="form1" runat="server">
      <asp:TextBox ID="TxtUid" runat="server"  TabIndex="1"   PlaceHolder="User Name" CssClass="Uidpwd"/>
            <asp:TextBox ID="TxtPwd" runat="server"  TextMode="Password" TabIndex="2"   PlaceHolder="Password" CssClass="Uidpwd"/>
               <asp:Button ID="BtnSubmit"  runat="server"  Text="Log in" TabIndex="3" CssClass="submit" />
 
  </form>
</div>
</body>
</html>

