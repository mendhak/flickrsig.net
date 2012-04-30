<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Manual.aspx.cs" Inherits="SeeMyMapWeb.Manual" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:TextBox TextMode="MultiLine" runat="server" ID="TextBoxXml" Height="178px"></asp:TextBox>
    </div>
   
    <div>
        <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" 
            onclick="ButtonSubmit_Click" />
    </div>
    
    </form>
</body>
</html>
