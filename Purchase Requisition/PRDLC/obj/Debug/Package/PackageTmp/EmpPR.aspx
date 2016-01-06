<%@ Page Title="Purchase Requisition" Language="vb" AutoEventWireup="false" MasterPageFile="~/EMPRDLC.Master"
    CodeBehind="EmpPR.aspx.vb" Inherits="PRDLC.EmpPR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function Confirmation() {
            if (confirm("Do you want to confirm?") == true) {
                return true;
            }
            else {
                return false;
            }
        }

        function Confirmation1() {
            if (confirm("Sure Want to withdraw the request?") == true) {
                return true;
            }
            else {
                return false;
            }
        }


        function AllowNumbers(el) {
            var ex = /^[0-9]+$/;
            if (ex.test(el.value) == false) {
                el.value = el.value.substring(0, el.value.length - 1);
            }
        }
        function alphanumerichypen(el) {
            var ex = /^[A-Za-z0-9_-]+$/;
            if (ex.test(el.value) == false) {
                el.value = el.value.substring(0, el.value.length - 1);
            }
        }

        function checkDec(el) {
            //            el.value = el.value.replace(/^[ 0]+/, '');
            var ex = /^\d*\.?\d{0,6}$/;
            if (ex.test(el.value) == false) {
                el.value = el.value.substring(0, el.value.length - 1);
            }
        }

        function RemoveZero(el) {
            el.value = el.value.replace(/^[ ]+/, '');
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

    </script>
    <script type="text/javascript">

        Sys.Application.add_load(ApplicationLoadHandler);

        function ApplicationLoadHandler(sender, args) {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (!prm.get_isInAsyncPostBack()) {
                prm.add_initializeRequest(initRequest);
                prm.add_endRequest(endRequest);
            }
        }

        function initRequest(sender, args) {
            var pop = $find("ModalPopupExtender6");
            pop.show();
        }

        function endRequest(sender, args) {
            var pop = $find("ModalPopupExtender6");
            pop.hide();
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
                            <asp:Label ID="Label3" runat="server" Text="Purchase Requisition" CssClass="subheader"
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
                                        <asp:ImageButton ID="btnhome" runat="server" ImageUrl="~/images/Homeicon.jpg" PostBackUrl="~/Home.aspx"
                                            ToolTip="Home" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="btnnew" runat="server" ImageUrl="~/images/Add.jpg" ToolTip="Add new record" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </asp:Panel>
                                    <asp:Label ID="Label2" runat="server" Text="" Style="color: Red;"></asp:Label>
                                    <asp:Panel ID="panelview" runat="server" Width="100%" BorderColor="LightSteelBlue"
                                        BorderWidth="2">
                                        <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                                            <tr>
                                                <td valign="top">
                                                    <ajx:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme"
                                                        Width="100%">
                                                        <ajx:TabPanel ID="TabPanel3" runat="server" HeaderText="Purchase Requisition">
                                                            <ContentTemplate>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="grdPurchaseRequest" runat="server" CellPadding="4" AllowPaging="True"
                                                                                ShowHeaderWhenEmpty="true" CssClass="mGrid" HeaderStyle-CssClass="GridBG" PagerStyle-CssClass="pgr"
                                                                                AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="false" Width="100%" PageSize="15">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Document Number">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:LinkButton ID="lbtndocnum" runat="server" Text='<%#Bind("DocEntry") %>' OnClick="lbtndocnum_Click"> <%----%>
                                                                                                </asp:LinkButton>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Employee No">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:Label ID="lblgtano" runat="server" Text='<%#Bind("U_Z_EmpID") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Employee Name">
                                                                                        <ItemTemplate>
                                                                                            <div align="center">
                                                                                                <asp:Label ID="lblgempid" runat="server" Text='<%#Bind("U_Z_EmpName") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Department ">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lbldept" runat="server" Text='<%#Bind("U_Z_DeptName") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Document Date">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblDocdate" runat="server" Text='<%#Bind("U_Z_DocDate") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Destination">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lbldesti" runat="server" Text='<%#Bind("U_Z_Destination") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Priority">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lblpriority" runat="server" Text='<%#Bind("U_Z_Priority") %>'></asp:Label>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Document Status">
                                                                                        <ItemTemplate>
                                                                                            <div align="left">
                                                                                                &nbsp;<asp:Label ID="lbldocStatus" runat="server" Text='<%#Bind("U_Z_DocStatus") %>'></asp:Label>
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
                                                    </ajx:TabContainer>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="PanelNewRequest" runat="server" Width="100%" BorderColor="LightSteelBlue"
                                        BorderWidth="2">
                                        <div id="Div1" runat="server" class="DivCorner" style="border: solid 2px LightSteelBlue;
                                            width: 100%;">
                                            <table width="99%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                                                <tr>
                                                    <td width="10%">
                                                        Document Number
                                                    </td>
                                                    <td width="15%">
                                                        <asp:Label ID="lbldocno" CssClass="txtbox" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="5%">
                                                    </td>
                                                    <td width="10%">
                                                        Document Date
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblsubdt" CssClass="txtbox" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="10%">
                                                        Employee No
                                                    </td>
                                                    <td width="10%">
                                                        <asp:Label ID="lblempNo" CssClass="txtbox" runat="server"></asp:Label>
                                                        <asp:Label ID="Label6" CssClass="txtbox" runat="server" Width="150px"></asp:Label>
                                                    </td>
                                                    <td width="5%">
                                                    </td>
                                                    <td width="10%">
                                                        Employee Name
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblempname" CssClass="txtbox" runat="server" Width="150px"></asp:Label>
                                                        <asp:Label ID="lblNExist" CssClass="txtbox" runat="server" Visible="false"></asp:Label>
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
                                                        Cost Center
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCostCenter" CssClass="txtbox" runat="server" Width="150px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Destination
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddldestination" CssClass="txtbox1" Width="160px" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="5%">
                                                    </td>
                                                    <td>
                                                        Priority
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlPriority" runat="server" CssClass="txtbox1">
                                                            <asp:ListItem Value="L">Low</asp:ListItem>
                                                            <asp:ListItem Value="M">Medium</asp:ListItem>
                                                            <asp:ListItem Value="H">High</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td width="5%">
                                                    </td>
                                                    <td width="10%">
                                                        Document Status
                                                    </td>
                                                    <td width="10%">
                                                        <asp:DropDownList ID="ddlNewStatus" runat="server" CssClass="txtbox1" Enabled="true">
                                                            <asp:ListItem Value="D">Draft</asp:ListItem>
                                                            <asp:ListItem Value="S">Confirm</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlDocStatus" runat="server" CssClass="txtbox1" Enabled="false">
                                                            <asp:ListItem Value="O">Open</asp:ListItem>
                                                            <asp:ListItem Value="I">InProgress</asp:ListItem>
                                                            <asp:ListItem Value="C">Closed</asp:ListItem>
                                                            <asp:ListItem Value="D">Draft</asp:ListItem>
                                                            <asp:ListItem Value="R">DLC Rejected</asp:ListItem>
                                                            <asp:ListItem Value="L">Cancelled</asp:ListItem>
                                                            <asp:ListItem Value="S">Confirm</asp:ListItem>
                                                            <asp:ListItem Value="DI">DLC InProgress</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:Panel ID="paltab" runat="server" Width="98%">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                                                <tr>
                                                    <td valign="top">
                                                        <ajx:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="ajax__tab_yuitabview-theme"
                                                            Width="100%">
                                                            <ajx:TabPanel ID="TabPanel5" runat="server" HeaderText="Purchase Requisition">
                                                                <ContentTemplate>
                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="4" class="main_content">
                                                                        <tr>
                                                                            <td id="NewExpense" runat="server">
                                                                                <asp:Button ID="lbtnNewExpenses" runat="server" CssClass="button" Text="Add New Request"
                                                                                    Style="float: left;" />
                                                                                <asp:Label ID="lblerror" runat="server" ForeColor="Red" CssClass="txtbox" Width="300px"></asp:Label>
                                                                                <ajx:ModalPopupExtender ID="ModalPopupExtender6" runat="server" PopupControlID="PanelItemEntry"
                                                                                    TargetControlID="lbtnNewExpenses" CancelControlID="btnclstech2" BackgroundCssClass="modalBackground"
                                                                                    DynamicServicePath="" Enabled="True">
                                                                                </ajx:ModalPopupExtender>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:GridView ID="grdPRequestLines" runat="server" CellPadding="4" CssClass="mGrid"
                                                                                    AutoGenerateColumns="False" Width="100%" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found">
                                                                                    <Columns>
                                                                                        <asp:CommandField ShowDeleteButton="True" />
                                                                                        <asp:TemplateField HeaderText="LineId" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblELineCode" runat="server" Text='<%#Bind("DocNum") %>'>
                                                                                                    </asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Ref. Code" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblERefCode" runat="server" Text='<%#Bind("RefNo") %>'>
                                                                                                    </asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Request Code" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblECode" runat="server" Text='<%#Bind("UniqueId") %>'>
                                                                                                    </asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="ItemCode">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblEitemcode" runat="server" Text='<%#Bind("ItemCode") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Item Description">
                                                                                            <ItemTemplate>
                                                                                                <div align="center">
                                                                                                    <asp:Label ID="lblEItemdesc" runat="server" Text='<%#Bind("ItemName") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Barcode" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblEbarcode" runat="server" Text='<%#Bind("Barcode") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Order Quantity">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblEOrderQty" runat="server" Text='<%#Bind("OrderQty") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Order UoM" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblEOrderUom" runat="server" Text='<%#Bind("OrderUoM") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Order UoM">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblEOrderUomDesc" runat="server" Text='<%#Bind("OrderUomDesc") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Alternate ItemCode">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblAItemCode" runat="server" Text='<%#Bind("AltItemCode") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Alternate ItemDesc">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblEAltItemdesc" runat="server" Text='<%#Bind("AltItemDesc") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Alternate Barcode" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblEAltbarcode" runat="server" Text='<%#Bind("AlterBarCode") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Delivered Qty">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblEDelQty" runat="server" Text='<%#Bind("DelQty") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Delivered UoM" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblEDelUom" runat="server" Text='<%#Bind("DelUom") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Delivered UoM">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblEDelUomDesc" runat="server" Text='<%#Bind("DelUomDesc") %>'></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Received Qty">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:TextBox ID="txtRecqty" Width="120px" runat="server" Text='<%#Bind("ReceivedQty") %>'
                                                                                                        onkeypress="AllowNumbers(this);checkDec(this);" onkeyup="AllowNumbers(this);checkDec(this);"></asp:TextBox>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Received Uom">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    &nbsp;<asp:Label ID="lblEReceiveUom" runat="server" Text='<%#Bind("ReceivedUom") %>'
                                                                                                        Visible="false"></asp:Label>
                                                                                                    &nbsp;<asp:DropDownList ID="ddlEReceUoM" runat="server">
                                                                                                    </asp:DropDownList>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Line Status">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    <asp:Label ID="Label5" runat="server" Text='<%#Bind("LineDesc") %>'></asp:Label>
                                                                                                    &nbsp;<asp:Label ID="lblELinestatus" runat="server" Text='<%#Bind("LineStatus") %>'
                                                                                                        Visible="false"></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Approval Status">
                                                                                            <ItemTemplate>
                                                                                                <div align="left">
                                                                                                    <asp:Label ID="Label4" runat="server" Text='<%#Bind("AppDesc") %>'></asp:Label>
                                                                                                    <asp:Label ID="lblEAppstatus" runat="server" Text='<%#Bind("AppStatus") %>' Visible="false"></asp:Label>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="imgbtn" Text="History" ImageUrl="~/Images/history-icon.png"
                                                                                                    ToolTip="View History" runat="server" Width="20" Height="20" OnClick="imgbtn_Click" />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <HeaderStyle BackColor="Gray" HorizontalAlign="Center" ForeColor="White" Height="25px"
                                                                                        CssClass="GridBG" />
                                                                                    <PagerStyle CssClass="pgr" />
                                                                                    <RowStyle HorizontalAlign="Left" CssClass="mousecursor" />
                                                                                    <AlternatingRowStyle HorizontalAlign="Left" CssClass="mousecursor" />
                                                                                </asp:GridView>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <br />
                                                                                <asp:Button ID="btnSubmit" CssClass="btn" Width="85px" runat="server" Text="Submit"
                                                                                    OnClientClick="return Confirmation();" />
                                                                                <asp:Button ID="btnRecSubmit" CssClass="btn" Width="85px" runat="server" Text="Submit"
                                                                                    OnClientClick="return Confirmation();" />
                                                                                <asp:Button ID="btnClose" CssClass="btn" Width="85px" runat="server" Text="Cancel" />
                                                                                <asp:Button ID="btnWithDraw" CssClass="btn" Width="182px" runat="server" Text="WithDraw Request"
                                                                                    OnClientClick="return Confirmation1();" />
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
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="PanelItemEntry" runat="server" BackColor="White" Style="display: none;
                            padding: 10px; width: 500px;">
                            <div>
                                <span class="sideheading" style="color: Green;">Purchase Request</span> <span style="float: right;">
                                    <asp:Button ID="btnclstech2" runat="server" CssClass="btn" Width="30px" Text="X" /></span></div>
                            <br />
                            <br />
                            <asp:Panel ID="Panel3" runat="server" Height="300px" ScrollBars="None">
                                <asp:Panel ID="Panel1" runat="server" Width="100%">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="3" class="main_content">
                                        <tr>
                                            <td width="20%">
                                                Item Code
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemcode" CssClass="txtbox" Width="210px" runat="server" Enabled="false"></asp:TextBox>
                                                <asp:ImageButton ID="btnItem" runat="server" Text="Find" ImageUrl="~/images/search.jpg" />
                                                <ajx:ModalPopupExtender ID="ModalPopupExtender7" runat="server" DropShadow="True"
                                                    PopupControlID="Panelpoptechnician" TargetControlID="btnItem" CancelControlID="btnclstech"
                                                    BackgroundCssClass="modalBackground">
                                                </ajx:ModalPopupExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10%">
                                                Item Description
                                            </td>
                                            <td width="30%">
                                                <asp:TextBox ID="txtitmdesc" CssClass="txtbox" Width="210px" Enabled="false" runat="server"></asp:TextBox>
                                                <asp:TextBox ID="txtIbarcode" CssClass="txtbox" Width="210px" runat="server" Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10%">
                                                UoM
                                            </td>
                                            <td width="30%">
                                                <asp:DropDownList ID="ddlUom" CssClass="txtbox1" Width="210px" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Ordered Quantity
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtorderqty" CssClass="txtbox" runat="server" autocomplete="off"
                                                    onkeypress="AllowNumbers(this);" onkeyup="AllowNumbers(this);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Order related to patients
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPatients" runat="server" CssClass="txtbox1">
                                                    <asp:ListItem Value="N">No</asp:ListItem>
                                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtitemspec" CssClass="txtbox" runat="server" Width="210px" Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <br />
                                                <asp:Button ID="btnAdd" CssClass="btn" Width="85px" runat="server" Text="Save" />
                                                <asp:Button ID="btncancel" CssClass="btn" Width="85px" runat="server" Text="Cancel"
                                                    Visible="false" />
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
                            <div>
                                <span>
                                    <asp:Label ID="lblitem" runat="server" Text="Item Code"></asp:Label></span>
                                <asp:TextBox ID="txtsearchItem" runat="server" Width="210px"></asp:TextBox>
                                <ajx:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtsearchItem"
                                    MinimumPrefixLength="1" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                    ServiceMethod="GetItems">
                                </ajx:AutoCompleteExtender>
                                <br />
                                <span>
                                    <asp:Label ID="lblitemNa" runat="server" Text="Item Name"></asp:Label></span>
                                <asp:TextBox ID="txtsearchItemNa" runat="server" Width="210px"></asp:TextBox>
                                <ajx:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="txtsearchItemNa"
                                    MinimumPrefixLength="1" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100"
                                    ServiceMethod="GetItemsName">
                                </ajx:AutoCompleteExtender>
                                <asp:Button ID="btngoItem" runat="server" Text="Go" CssClass="button" Width="50px" />
                                <asp:LinkButton ID="lbtpopnview" runat="server">View All</asp:LinkButton>
                                <br />
                            </div>
                            <br />
                            <asp:Panel ID="Panel4" runat="server" Height="200px" ScrollBars="Vertical">
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
                        <div style="visibility: hidden">
                            <asp:Button ID="btnSample" runat="server" />
                        </div>
                        <ajx:ModalPopupExtender ID="ModalPopupExtender2" runat="server" DropShadow="True"
                            PopupControlID="Panelpoptechnician1" TargetControlID="btnSample" CancelControlID="btnclstech2"
                            BackgroundCssClass="modalBackground">
                        </ajx:ModalPopupExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panelpoptechnician1" runat="server" BackColor="White" Style="display: none;
                            padding: 10px; width: 900px;">
                            <div>
                                <span class="sideheading" style="color: Green;">Approval History Details</span>
                                <span style="float: right;">
                                    <asp:Button ID="Button1" runat="server" CssClass="btn" Width="30px" Text="X" /></span></div>
                            <asp:Panel ID="Panel5" runat="server" Height="400px" ScrollBars="Vertical">
                                <asp:Label ID="Label1" runat="server" Text="" CssClass="txtbox" ForeColor="Red"></asp:Label>
                                <asp:GridView ID="grdRequesttohr" runat="server" CellPadding="4" ShowHeaderWhenEmpty="true"
                                    CssClass="mGrid" HeaderStyle-CssClass="GridBG" AlternatingRowStyle-CssClass="alt"
                                    AutoGenerateColumns="false" Width="100%">
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
                                    <HeaderStyle HorizontalAlign="Center" Height="25px" BackColor="#CCCCCC" />
                                </asp:GridView>
                            </asp:Panel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <%-- <asp:PostBackTrigger ControlID="Panel1$btnAdd" />--%>
            <asp:AsyncPostBackTrigger ControlID="Panel3$Panel1$btnAdd" EventName="Click" />
            <%--   <asp:PostBackTrigger ControlID="TabContainer1$TabPanel5$btnSubmit" />
            <asp:PostBackTrigger ControlID="TabContainer1$TabPanel5$btnClose" />
             <asp:PostBackTrigger ControlID="TabContainer1$TabPanel5$btnRecSubmit" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
