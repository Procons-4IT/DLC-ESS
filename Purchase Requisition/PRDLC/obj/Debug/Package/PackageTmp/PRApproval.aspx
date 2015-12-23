<%@ Page Title="Purchase Requisition Approval" Language="vb" AutoEventWireup="false"
    MasterPageFile="~/EMPRDLC.Master" CodeBehind="PRApproval.aspx.vb"
    Inherits="PRDLC.PRApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function Confirmation() {
            if (confirm("Are you sure want to submit the document?") == true) {
                return true;
            }
            else {
                return false;
            }
        }

        function Confirmation1() {
            if (confirm("Are you sure want to Cancel the document?") == true) {
                return true;
            }
            else {
                return false;
            }
        }

        function popupdisplay(option, uniqueid, tripno, CouName) {
            if (uniqueid.length > 0) {
                var uniid = document.getElementById("<%=txtpopunique.ClientID%>").value;
                var tno = document.getElementById("<%=txtpoptno.ClientID%>").value;
                var opt = document.getElementById("<%=txthidoption.ClientID%>").value;
                var tName = document.getElementById("<%=txthidoption.ClientID%>").value;
                uniid = ""; tno = ""; opt = ""; tName = "";
                if (uniid != uniqueid && tno != tripno && opt != option && tName != CouName) {
                    document.getElementById("<%=txtpopunique.ClientID%>").value = uniqueid;
                    document.getElementById("<%=txtpoptno.ClientID%>").value = tripno;
                    document.getElementById("<%=txthidoption.ClientID%>").value = option;
                    document.getElementById("<%=txttname.ClientID%>").value = CouName;
                    document.getElementById("<%=Btncallpop.ClientID%>").onclick();
                }
            }
        }



        function SelectAllCheckboxes(spanChk) {

            // Added as ASPX uses SPAN for checkbox
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ?
        spanChk : spanChk.children.item[0];
            xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++)
                if (elm[i].type == "checkbox" &&
              elm[i].id != theBox.id) {
                    //elm[i].click();
                    if (elm[i].checked != xState)
                        elm[i].click();
                    //elm[i].checked=xState;
                }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="../Images/waiting.gif" AlternateText="Processing"
                runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <ajx:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
    <asp:UpdatePanel ID="Update" runat="server">
        <ContentTemplate>
            <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                <tr>
                    <td>
                        <asp:TextBox ID="txtHEmpID" runat="server" Width="93px" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="txtpopunique" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="txtpoptno" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="txthidoption" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="txttname" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="txtbarcode" runat="server" Style="display: none"></asp:TextBox>
                        <input id="Btncallpop" runat="server" onserverclick="Btncallpop_ServerClick" style="display: none"
                            type="button" value="button" />
                    </td>
                </tr>
                <tr>
                    <td height="30" align="left" colspan="2" valign="bottom" background="images/h_bg.png"
                        style="border-bottom: 1px dotted; border-color: #f45501; background-repeat: repeat-x">
                        <div>
                            &nbsp;
                            <asp:Label ID="Label3" runat="server" Text="Purchase Requisition Approval" CssClass="subheader"
                                Style="float: left;"></asp:Label>
                            <span>
                                <asp:Label ID="lblNewTrip" runat="server" Text="" Visible="false"></asp:Label></span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                            <tr>
                                <td>
                                    <asp:Panel ID="panelhome" runat="server" Width="100%">
                                        <asp:ImageButton ID="btnhome" runat="server" ImageUrl="~/images/Homeicon.jpg" PostBackUrl="Home.aspx"
                                            ToolTip="Home" />
                                    </asp:Panel>
                                    <asp:Panel ID="PanelMain" runat="server" Width="100%" BorderColor="LightSteelBlue"
                                        BorderWidth="2">
                                        <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                                            <tr>
                                                <td valign="top">
                                                    <ajx:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme"
                                                        Width="100%">
                                                        <ajx:TabPanel ID="TabPanel2" runat="server" HeaderText="Purchase Requisition Approval">
                                                            <ContentTemplate>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="GrdLoadRequest" runat="server" CellPadding="4" AllowPaging="True"
                                                                                ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="mGrid"
                                                                                HeaderStyle-CssClass="GridBG" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                                                AutoGenerateColumns="false" Width="100%" PageSize="15">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Request Code">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:LinkButton ID="lblRCode" runat="server" Text='<%#Bind("Code") %>' OnClick="lnbtnlblRCode_Click"> 
                                                                                                </asp:LinkButton>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Emp.Code">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:Label ID="lblREmpid" runat="server" Text='<%#Bind("U_Z_EmpID") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Emp.Name">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:Label ID="lblREmpname" runat="server" Text='<%#Bind("U_Z_EmpName") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Document Date">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblRSubDt" runat="server" Text='<%#Bind("U_Z_DocDate") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Department">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblRDept" runat="server" Text='<%#Bind("U_Z_DeptName") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Warehouse">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblRwhs" runat="server" Text='<%#Bind("U_Z_Destination") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Document Status">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblRDocStatus" runat="server" Text='<%#Bind("U_Z_DocStatus") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <HeaderStyle HorizontalAlign="Center" Height="25px" BackColor="#CCCCCC" />
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </ajx:TabPanel>
                                                        <ajx:TabPanel ID="TabPanel4" runat="server" HeaderText="Purchase Requisition Summary">
                                                            <ContentTemplate>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="grdSummaryLoad" runat="server" CellPadding="4" AllowPaging="True"
                                                                                ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="mGrid"
                                                                                HeaderStyle-CssClass="GridBG" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                                                AutoGenerateColumns="false" Width="100%" PageSize="15">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Request Code">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:LinkButton ID="lblSCode" runat="server" Text='<%#Bind("Code") %>' OnClick="lnbtnlblSCode_Click"> 
                                                                                                </asp:LinkButton>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Emp.Code">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:Label ID="lblSEmpid" runat="server" Text='<%#Bind("U_Z_EmpID") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Emp.Name">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:Label ID="lblSEmpname" runat="server" Text='<%#Bind("U_Z_EmpName") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Document Date">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblSSubDt" runat="server" Text='<%#Bind("U_Z_DocDate") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Department">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblSDept" runat="server" Text='<%#Bind("U_Z_DeptName") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Warehouse">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblSwhs" runat="server" Text='<%#Bind("U_Z_Destination") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Document Status">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblSDocStatus" runat="server" Text='<%#Bind("U_Z_DocStatus") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="lbtnHistory" runat="server" Text="Add New Expenses" Style="display: none;" />
                                                                            <ajx:ModalPopupExtender ID="ModalPopupExtender6" runat="server" DropShadow="false"
                                                                                PopupControlID="PanelItemEntry" TargetControlID="lbtnHistory" CancelControlID="btnclstech2"
                                                                                BackgroundCssClass="modalBackground">
                                                                            </ajx:ModalPopupExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <%--  <a class="txtbox" style=" text-decoration:underline; font-weight:bold; width:300px;">Purchase Requisition Details</a>--%>
                                                                            <br />
                                                                            <div style="overflow-x: scroll; width: 1100px;">
                                                                                <asp:GridView ID="grdSummary" runat="server" CellPadding="4" ShowHeaderWhenEmpty="true"
                                                                                    EmptyDataText="No Records Found" CssClass="mGrid" HeaderStyle-CssClass="GridBG"
                                                                                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="false"
                                                                                    Width="100%">
                                                                                    <Columns>
                                                                                     <asp:TemplateField HeaderText="Code" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblSlCode" runat="server" Text='<%#Bind("DocEntry") %>'> <%--OnClick="lbtnlblSCode_Click"--%>
                                                                                                    </asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Code">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:LinkButton ID="lblSCode" runat="server" Text='<%#Bind("LineId") %>' OnClick="lbtnlblSCode_Click">
                                                                                                    </asp:LinkButton>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Emp.Code" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblsEmpid" runat="server" Text='<%#Bind("U_Z_EmpID") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Emp.Name" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblsEmpname" runat="server" Text='<%#Bind("U_Z_EmpName") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Document Date" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsAgenda" runat="server" Text='<%#Bind("U_Z_DocDate") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="DeptCode" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsdeptcode" runat="server" Text='<%#Bind("U_Z_DeptCode") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="DeptName" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsdeptname" runat="server" Text='<%#Bind("U_Z_DeptName") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Warehouse" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblswhss" runat="server" Text='<%#Bind("U_Z_Destination") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="ItemCode">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsItemCode" runat="server" Text='<%#Bind("U_Z_ItemCode") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Item Name">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsItemName" runat="server" Text='<%#Bind("U_Z_ItemName") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    
                                                                                        <asp:TemplateField HeaderText="Ordered Quantity">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsordQty" runat="server" Text='<%#Bind("U_Z_OrdQty") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Order UoM">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsordUom" runat="server" Text='<%#Bind("U_Z_OrdUomDesc") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Alternate itemCode">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsaltItemCode" runat="server" Text='<%#Bind("U_Z_AltItemCode") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Alternate ItemName">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsitdesc" runat="server" Text='<%#Bind("U_Z_AltItemName") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Delivered Qty">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsdelqty" runat="server" Text='<%#Bind("U_Z_DeliQty") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Delivered UoM">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsdelUom" runat="server" Text='<%#Bind("U_Z_DelUomDesc") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Line Status" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsLinests" runat="server" Text='<%#Bind("U_Z_LineStatus") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Goods Issue">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsgoods" runat="server" Text='<%#Bind("U_Z_DocNo") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approval Status">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsAppStatus" runat="server" Text='<%#Bind("U_Z_AppStatus") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approval Required" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsAppreq" runat="server" Text='<%#Bind("U_Z_AppRequired") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Requested Date">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsreqdt" runat="server" Text='<%#Bind("U_Z_AppReqDate") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Requested Time" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsreqtime" runat="server" Text='<%#Bind("U_Z_ReqTime") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Current Approver">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblscurapp" runat="server" Text='<%#Bind("U_Z_CurApprover") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Next Approver">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsnxtapp" runat="server" Text='<%#Bind("U_Z_NxtApprover") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <HeaderStyle HorizontalAlign="Center" Height="25px" BackColor="#CCCCCC" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </ajx:TabPanel>
                                                    </ajx:TabContainer>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="panelview" runat="server" Width="100%" BorderColor="LightSteelBlue"
                                        BorderWidth="2">
                                        <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                                            <tr>
                                                <td>
                                                    <div id="Div1" runat="server" class="DivCorner" style="border: solid 2px LightSteelBlue;
                                                        width: 100%;">
                                                        <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                                                            <tr>
                                                                <td width="10%">
                                                                    Document Number
                                                                </td>
                                                                <td width="15%">
                                                                    <asp:Label ID="lbldocno" CssClass="txtbox" Width="200px" runat="server"></asp:Label>
                                                                </td>
                                                                <td width="5%">
                                                                </td>
                                                                <td width="10%">
                                                                    Document Date
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblsubdt" CssClass="txtbox" Width="200px" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="10%">
                                                                    Employee No
                                                                </td>
                                                                <td width="10%">
                                                                    <asp:Label ID="lblempNo" CssClass="txtbox" runat="server"></asp:Label>
                                                                </td>
                                                                <td width="5%">
                                                                </td>
                                                                <td width="10%">
                                                                    Employee Name
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblempname" CssClass="txtbox" runat="server" Width="150px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    Department
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lbldeptName" CssClass="txtbox" runat="server" Width="150px"></asp:Label>
                                                                    <asp:Label ID="lbldept" CssClass="txtbox" runat="server" Visible="false"></asp:Label>
                                                                </td>
                                                                <td width="5%">
                                                                </td>
                                                                <td width="10%">
                                                                    Destination
                                                                </td>
                                                                <td width="10%">
                                                                    <asp:DropDownList ID="ddldestination" CssClass="txtbox1" Width="210px" runat="server"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="10%">
                                                                </td>
                                                                <td width="10%">
                                                                    <asp:Label ID="lblTANo" CssClass="txtbox" runat="server" Visible="false"></asp:Label>
                                                                </td>
                                                                <td width="5%">
                                                                </td>
                                                                <td width="10%">
                                                                    Document Status
                                                                </td>
                                                                <td width="10%">
                                                                    <asp:Label ID="lblDocStatus" CssClass="txtbox" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <ajx:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme"
                                                        Width="100%">
                                                        <ajx:TabPanel ID="TabPanel3" runat="server" HeaderText="Purchase Requisition Approval">
                                                            <ContentTemplate>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                                                            <ajx:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                                                                                PopupControlID="Panel2" TargetControlID="btnShowPopup" CancelControlID="btnclsitem1"
                                                                                BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True">
                                                                            </ajx:ModalPopupExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div style="overflow-x: scroll; width: 1100px;">
                                                                                <asp:GridView ID="grdRequestApproval" runat="server" CellPadding="4" ShowHeaderWhenEmpty="True"
                                                                                    EmptyDataText="No Records Found" CssClass="mGrid" AutoGenerateColumns="False"
                                                                                    Width="100%">
                                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Request No." Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:LinkButton ID="lblRefCode" runat="server" Text='<%#Bind("DocEntry") %>'>
                                                                                                    </asp:LinkButton>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Select">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:CheckBox ID="chkGoods" runat="server"></asp:CheckBox>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                            <HeaderTemplate>
                                                                                                <input id="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                                                                                                    type="checkbox" />
                                                                                            </HeaderTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Serial No.">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:LinkButton ID="lblCode" runat="server" Text='<%#Bind("LineId") %>' OnClick="lbtnlblCode_Click"> <%-- --%>
                                                                                                    </asp:LinkButton>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Emp.Code" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblEmpid" runat="server" Text='<%#Bind("U_Z_EmpID") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Emp.Name" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblEmpname" runat="server" Text='<%#Bind("U_Z_EmpName") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Document Date" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblAgenda" runat="server" Text='<%#Bind("U_Z_DocDate") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="ItemCode">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsItemCode" runat="server" Text='<%#Bind("U_Z_ItemCode") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Item Name">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsItemName" runat="server" Text='<%#Bind("U_Z_ItemName") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                      
                                                                                        <asp:TemplateField HeaderText="Ordered Quantity">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsordQty" runat="server" Text='<%#Bind("U_Z_OrdQty") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Order UoM">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsordUom" runat="server" Text='<%#Bind("U_Z_OrdUomDesc") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Alternate itemCode">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsaltItemCode" runat="server" Text='<%#Bind("U_Z_AltItemCode") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Alternate ItemName">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsitdesc" runat="server" Text='<%#Bind("U_Z_AltItemName") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Delivered Qty">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsdelqty" runat="server" Text='<%#Bind("U_Z_DeliQty") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Delivered UoM">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsdelUom" runat="server" Text='<%#Bind("U_Z_DelUomDesc") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Delivered UoM Desc" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsdelUomdesc" runat="server" Text='<%#Bind("U_Z_DelUomDesc") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Line Status" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsLinests" runat="server" Text='<%#Bind("U_Z_LineStatus") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approval Status" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblAppStatus" runat="server" Text='<%#Bind("U_Z_AppStatus") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Remarks" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:TextBox ID="txtRemarks" runat="server" Text='<%#Bind("U_Z_RejRemark") %>'></asp:TextBox>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approval Required" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblAppreq" runat="server" Text='<%#Bind("U_Z_AppRequired") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Requested Date" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblreqdt" runat="server" Text='<%#Bind("U_Z_AppReqDate") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Requested Time" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblreqtime" runat="server" Text='<%#Bind("U_Z_ReqTime") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Current Approver">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblcurapp" runat="server" Text='<%#Bind("U_Z_CurApprover") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Next Approver">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblnxtapp" runat="server" Text='<%#Bind("U_Z_NxtApprover") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Doc.No">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsgoods" runat="server" Text='<%#Bind("U_Z_DocNo") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="imgbtn" Text="Edit" ImageUrl="~/Images/edit-icon.png" ToolTip="Edit"
                                                                                                    runat="server" Width="20" Height="20" OnClick="imgbtn_Click" />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="btndelete" Text="Cancel" runat="server" ImageUrl="~/Images/Cancel.png"
                                                                                                    Width="20" Height="20" ToolTip="Cancel" OnClick="btndelete_Click" OnClientClick="return Confirmation1();" />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <HeaderStyle HorizontalAlign="Center" Height="25px" BackColor="#CCCCCC" CssClass="GridBG" />
                                                                                    <PagerStyle CssClass="pgr" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <div style="overflow-x: scroll; width: 700px;">
                                                                                <a class="txtbox" style="text-decoration: underline; font-weight: bold;">Approval History</a>
                                                                                <asp:GridView ID="grdApprovalHis" runat="server" CellPadding="4" AllowPaging="True"
                                                                                    ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" CssClass="mGrid"
                                                                                    AutoGenerateColumns="False" Width="100%">
                                                                                    <AlternatingRowStyle CssClass="alt" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Request Code" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblHDocNo" runat="server" Text='<%#Bind("DocEntry") %>'>
                                                                                                    </asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Reference No" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblhrefno" runat="server" Text='<%#Bind("U_Z_DocEntry") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="DocType" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblhdoctype" runat="server" Text='<%#Bind("U_Z_DocType") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Employee ID">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhempid" runat="server" Text='<%#Bind("U_Z_EmpId") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Employee Name">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhempname" runat="server" Text='<%#Bind("U_Z_EmpName") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approved By">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhAppby" runat="server" Text='<%#Bind("U_Z_ApproveBy") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="ItemCode">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsahItemCode" runat="server" Text='<%#Bind("U_Z_ItemCode") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Item Name">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsahItemName" runat="server" Text='<%#Bind("U_Z_ItemName") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Ordered Quantity">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsahordQty" runat="server" Text='<%#Bind("U_Z_OrdQty") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Order UoM">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblsahordUom" runat="server" Text='<%#Bind("U_Z_delUomDesc") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Create Date">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhcrdate" runat="server" Text='<%#Bind("CreateDate") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Create Time">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhcrtime" runat="server" Text='<%#Bind("CreateTime") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Update Date">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhupdate" runat="server" Text='<%#Bind("UpdateDate") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Update Time">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhuptime" runat="server" Text='<%#Bind("UpdateTime") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approved Status">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhAppstatus" runat="server" Text='<%#Bind("U_Z_AppStatus") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Remarks">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblhremarks" runat="server" Text='<%#Bind("U_Z_Remarks") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <HeaderStyle HorizontalAlign="Center" Height="25px" BackColor="#CCCCCC" CssClass="GridBG" />
                                                                                    <PagerStyle CssClass="pgr" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                        <td valign="top">
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <br />
                                                                                        <asp:Button ID="btnAdd" runat="server" CssClass="btn" OnClientClick="return Confirmation();"
                                                                                            Text="Save &amp; Submit" Width="125px" />
                                                                                        <asp:Button ID="btncancel" runat="server" CssClass="btn" Text="Cancel" Width="85px" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </ajx:TabPanel>
                                                    </ajx:TabContainer>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="PanelItemEntry" runat="server" BackColor="White" Style="display: none;
                            padding: 10px; width: 1000px;">
                            <div>
                                <span style="float: right;">
                                    <asp:Button ID="btnclstech2" runat="server" CssClass="btn" Width="30px" Text="X" /></span></div>
                            <br />
                            <asp:Panel ID="Panel3" runat="server" Height="400px">
                                <div style="overflow-y: scroll;">
                                    <asp:Panel ID="Panel1" runat="server" Width="100%">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                            <tr>
                                                <td>
                                                    <a class="txtbox" style="text-decoration: underline; font-weight: bold;">Approval History</a>
                                                    <br />
                                                    <asp:GridView ID="grdHistorySummary" runat="server" CellPadding="4" AllowPaging="True"
                                                        ShowHeaderWhenEmpty="true" EmptyDataText="No Records Found" CssClass="mGrid"
                                                        HeaderStyle-CssClass="GridBG" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                        AutoGenerateColumns="false" Width="100%" PageSize="10">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Request Code" Visible="false">
                                                                <ItemTemplate>
                                                                    <div align="center">
                                                                        <asp:Label ID="lblDocNo" runat="server" Text='<%#Bind("DocEntry") %>'>
                                                                        </asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Reference No" Visible="false">
                                                                <ItemTemplate>
                                                                    <div align="center">
                                                                        <asp:Label ID="lblrefno" runat="server" Text='<%#Bind("U_Z_DocEntry") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DocType" Visible="false">
                                                                <ItemTemplate>
                                                                    <div align="center">
                                                                        <asp:Label ID="lbldoctype" runat="server" Text='<%#Bind("U_Z_DocType") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Employee ID">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblempid" runat="server" Text='<%#Bind("U_Z_EmpId") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Employee Name">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblempname" runat="server" Text='<%#Bind("U_Z_EmpName") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Approved By">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblAppby" runat="server" Text='<%#Bind("U_Z_ApproveBy") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ItemCode">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblshItemCode" runat="server" Text='<%#Bind("U_Z_ItemCode") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item Name">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblshItemName" runat="server" Text='<%#Bind("U_Z_ItemName") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ordered Quantity">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblshordQty" runat="server" Text='<%#Bind("U_Z_OrdQty") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Order UoM">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblshordUom" runat="server" Text='<%#Bind("U_Z_delUomDesc") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Create Date">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblcrdate" runat="server" Text='<%#Bind("CreateDate") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Create Time" Visible="false">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblcrtime" runat="server" Text='<%#Bind("CreateTime") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Update Date">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblupdate" runat="server" Text='<%#Bind("UpdateDate") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Update Time" Visible="false">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lbluptime" runat="server" Text='<%#Bind("UpdateTime") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Approved Status">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblAppstatus" runat="server" Text='<%#Bind("U_Z_AppStatus") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remarks">
                                                                <ItemTemplate>
                                                                    <div align="left">
                                                                        &nbsp;<asp:Label ID="lblremarks" runat="server" Text='<%#Bind("U_Z_Remarks") %>'></asp:Label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Center" Height="25px" BackColor="#CCCCCC" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>
                            </asp:Panel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel2" runat="server" BackColor="White" Style="display: none; padding: 10px;
                            width: 500px;">
                            <div>
                                <span class="sideheading" style="color: Green;">Purchase Request</span> <span style="float: right;">
                                    <asp:Button ID="btnclsitem1" runat="server" CssClass="btn" Width="30px" Text="X" /></span></div>
                            <br />
                            <br />
                            <asp:Panel ID="Panel4" runat="server" Height="500px" ScrollBars="None">
                                <asp:Panel ID="Panel5" runat="server" Width="100%">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                        <tr>
                                            <td width="15%">
                                                Item Code
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemcode" CssClass="txtbox" Width="210px" runat="server" Enabled="false"></asp:TextBox>
                                               <%-- <asp:ImageButton ID="btnItem" runat="server" Text="Find" ImageUrl="~/images/search.jpg"
                                                    Visible="false" />
                                                <ajx:ModalPopupExtender ID="ModalPopupExtender7" runat="server" DropShadow="True"
                                                    PopupControlID="Panelpoptechnician1" TargetControlID="btnItem" CancelControlID="btnclstech1"
                                                    BackgroundCssClass="modalBackground">
                                                </ajx:ModalPopupExtender>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10%">
                                                Item Description
                                            </td>
                                            <td width="30%">
                                                <asp:TextBox ID="txtitmdesc" CssClass="txtbox" Width="210px" Enabled="false" runat="server"></asp:TextBox>
                                                <asp:Label ID="lblCode" CssClass="txtbox" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10%">
                                                Item BarCode
                                            </td>
                                            <td width="30%">
                                                <asp:TextBox ID="txtIbarcode" CssClass="txtbox" runat="server" Width="210px" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                       
                                        <tr>
                                            <td width="10%">
                                                UoM
                                            </td>
                                            <td width="30%">
                                                <asp:DropDownList ID="ddlUom" CssClass="txtbox1" Width="210px" runat="server" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Ordered Quantity
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtorderqty" CssClass="txtbox" runat="server" autocomplete="off"
                                                    Enabled="false" Width="210px" AutoPostBack="true" onkeypress="AllowNumbers(this);checkDec(this);"
                                                    onkeyup="AllowNumbers(this);checkDec(this);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10%">
                                                Alternate Item Code
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAltItemCode" CssClass="txtbox" Width="210px" runat="server" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="btnAltItem" runat="server" Text="Find" ImageUrl="~/images/search.jpg" />
                                                <ajx:ModalPopupExtender ID="ModalPopupExtender2" runat="server" DropShadow="True"
                                                    PopupControlID="Panelpoptechnician" TargetControlID="btnAltItem" CancelControlID="btnclstech"
                                                    BackgroundCssClass="modalBackground">
                                                </ajx:ModalPopupExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10%">
                                                Alternate Item Description
                                            </td>
                                            <td width="30%">
                                                <asp:TextBox ID="txtItemdesc" CssClass="txtbox" Width="210px" Enabled="false" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Alternate Item BarCode
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAltBarCode" CssClass="txtbox" runat="server" Width="210px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10%">
                                                Delivered UoM
                                            </td>
                                            <td width="30%">
                                                <asp:DropDownList ID="ddldelUoM" CssClass="txtbox1" Width="210px" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Ordered Quantity
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDelQty" CssClass="txtbox" runat="server" autocomplete="off" Width="210px"
                                                    onkeypress="AllowNumbers(this);checkDec(this);" onkeyup="AllowNumbers(this);checkDec(this);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Approval Status
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlAppStatus" CssClass="txtbox1" Width="210px" runat="server">
                                                    <asp:ListItem Value="O">Open</asp:ListItem>
                                                    <asp:ListItem Value="D">Approved</asp:ListItem>
                                                    <asp:ListItem Value="L">Cancelled</asp:ListItem>
                                                    <asp:ListItem Value="C">Close</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Remarks
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRejRemarks" CssClass="txtbox" runat="server" Width="210px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <br />
                                                <asp:Button ID="btnSubmit" CssClass="btn" Width="85px" runat="server" Text="Save" />
                                                <asp:Button ID="Button3" CssClass="btn" Width="85px" runat="server" Text="Cancel"
                                                    Visible="false" />
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                               
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemSpec" CssClass="txtbox" runat="server" Width="210px" Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panelpoptechnician" runat="server" BackColor="White" Style="display: none;
                            padding: 10px; width: 500px;">
                            <div>
                                <span class="sideheading" style="color: Green;">Item Details</span> <span style="float: right;">
                                    <asp:Button ID="btnclstech" runat="server" CssClass="btn" Width="30px" Text="X" /></span></div>
                            <br />
                            <br />
                            <asp:Panel ID="Panel7" runat="server" Height="200px" ScrollBars="Vertical">
                                <asp:GridView ID="grdItems" runat="server" CellPadding="4" RowStyle-CssClass="mousecursor"
                                    CssClass="mGrid" AutoGenerateColumns="false" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Item Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemcode" runat="server" Text='<%#Bind("ItemCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ItemName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemname" runat="server" Text='<%#Bind("ItemName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BarCode">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfrgnname" runat="server" Text='<%#Bind("CodeBars") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="Gray" HorizontalAlign="Center" ForeColor="White" Height="25px" />
                                </asp:GridView>
                            </asp:Panel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panelpoptechnician1" runat="server" BackColor="White" Style="display: none;
                            padding: 10px; width: 500px;">
                            <div>
                                <span class="sideheading" style="color: Green;">Item Details</span> <span style="float: right;">
                                    <asp:Button ID="btnclstech1" runat="server" CssClass="btn" Width="30px" Text="X" /></span></div>
                            <br />
                            <br />
                            <asp:Panel ID="Panel6" runat="server" Height="200px" ScrollBars="Vertical">
                                <asp:GridView ID="grdItems1" runat="server" CellPadding="4" RowStyle-CssClass="mousecursor"
                                    CssClass="mGrid" AutoGenerateColumns="false" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Item Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemcode" runat="server" Text='<%#Bind("ItemCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ItemName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemname" runat="server" Text='<%#Bind("ItemName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BarCode">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfrgnname" runat="server" Text='<%#Bind("CodeBars") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="Gray" HorizontalAlign="Center" ForeColor="White" Height="25px" />
                                </asp:GridView>
                            </asp:Panel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
