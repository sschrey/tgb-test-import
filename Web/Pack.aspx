<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pack.aspx.cs" Inherits="Web.Pack" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<input type="button" value="All 0" accesskey="0" style="float: right" onclick="Zeroize()" />
<div style="clear:both"/>

<asp:Panel ID="pnlOrders" runat="server">
    <table class="ListOrders" width="800" id="OrderLines">
        <thead>
            <tr>
                <th>Line</th>
                <th>Part</th>
                <th>Qty</th>
                <th>Part weight</th>
                <th>Packs</th>
                <th>
                
                <asp:DropDownList ID="ddlContainers" runat="server" DataTextField="Name" DataValueField="Id" />
                
               </th>
                
            </tr>
        </thead>

    <asp:Repeater runat="server" ID="rptOrderLines">
        
        <ItemTemplate>
            <tr>
                
                <td width="100">
                    <asp:Label runat="server" ID="labLineNumber" Text='<%# Eval("Id") %>' />
                </td>
                <td width="400">
                    <asp:Label runat="server" ID="labPartNumber" Text='<%# Eval("PartId") %>' />
                    <br />
                    <asp:Label runat="server" ID="labPartName" Text='<%# Eval("PartName") %>' />
                    <br />
                </td>
                <td width="75">
                    <asp:Label runat="server" ID="labQuantity" Text='<%# Eval("OrderQty") %>' />
                </td>
                <td width="75">
                    <asp:Label runat="server" ID="lblWeight" Text='<%# Eval("PartWeight") %>' />gr                   
                </td>
                <td>
                    <asp:Repeater runat="server" ID="rptPacked" DataSource='<%# Eval("Packs") %>'>
                        <ItemTemplate>
                            <p>
                            
                                [<asp:Literal runat="server" ID="Literal0" Text='<%# Eval("PackedContainer.Container.Name") %>' />]
                                x
                                [<asp:Literal runat="server" ID="Literal1" Text='<%# Eval("Qty") %>' />]</p>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPacked" Columns="8" OnKeyDown="return EnsureNumeric(event)"
                        Text='<%# Eval("PackQty") %>' Enabled='<%# (int)(Eval("PackQty"))>0?true:false  %>' />
                    <asp:RangeValidator runat="server" ID="val1" ControlToValidate="txtPacked" Display="Dynamic"
                        Type="Integer" MinimumValue="0" MaximumValue='<%# Eval("PackQty") %>' Text="Too many" />
                </td>
            </tr>
        </ItemTemplate>
        
    </asp:Repeater>
    </table>
</asp:Panel>

<asp:Button runat="server" ID="btnPack" OnClick="btnPack_Click" Text="PACK" />

<asp:Panel Visible="false" ID="pnlPacks" runat="server">

    <asp:Repeater runat="server" ID="rptPacks">
        <HeaderTemplate>
            <table class="ListOrders">
                <thead>
                <tr>
                    <th>
                        Package code
                    </th>
                    <th>
                        Container name
                    </th>
                    <th>Estimated weight</th>
                    <th>Weight</th>
                    <th>
                        Containing products
                    </th>
                </tr>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("Key.Id") %>
                </td>
                <td>
                    <asp:HiddenField ID="packedContainerId" runat="server" Value='<%# Eval("Key.Id") %>' />
                    <%# Eval("Key.Container.Name") %>
                    [<%# Eval("Key.Container.Weight") %>gr]
                </td>
                <td>
                <asp:HiddenField ID="EstimatedWeight" runat="server" Value='<%# Eval("Key.EstimatedWeight") %>' />
                <%# Eval("Key.EstimatedWeightAsString") %>
                </td>
                <td>
                    <asp:Label ID="lblWeight" Visible='<%# (int)Eval("Key.Weight") > 0 %>' runat="server" Text='<%# Eval("Key.Weight") %>' />
                    <asp:TextBox ID="tbWeight" Visible='<%# (int)Eval("Key.Weight") == 0 %>' runat="server" Text='<%# Eval("Key.Weight") %>' /> gr
                    <asp:RangeValidator ID="rngWeight" runat="server" ControlToValidate="tbWeight"
                    MinimumValue="1" Type="Integer" Display="Dynamic" MaximumValue="10000000" ErrorMessage="Please enter more than 0" />
                    
                    
                </td>
                <td>
                    <asp:Repeater id="rptParts" runat="server" DataSource='<%# Eval("Value") %>'>
                        <ItemTemplate>
                            Part <%# Eval("Key") %>, qty:
                            <%# Eval("Value") %><br />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="SAVE"
        OnClientClick="if (!Page_ClientValidate()){ return false; } this.disabled = true; this.value = 'Saving...';" 
   UseSubmitBehavior="false" />
</asp:Panel>


<asp:Button Visible="false" runat="server" ID="btnShip" OnClick="btnShip_Click" Text="SHIP" />

<ajaxToolkit:ModalPopupExtender ID="mpPopup" runat="server"
                                PopupControlID="pnlPopup"
                                TargetControlID="hiddenTarget"
                                CancelControlID="btnCancelPopup"
                                BackgroundCssClass="modalBackground" 
                                />

<asp:HiddenField ID="hiddenTarget" runat="server" />  
<asp:Panel CssClass="modalPopup" ID="pnlPopup" runat="server" style="display:none">
    <asp:HiddenField ID="action" runat="server" />
    <asp:Label EnableViewState="false" CssClass="modelPopupTitle" ID="lblPopupTitle" runat="server"
               Text="" /><br /><br />
    <asp:Button ID="btnConfirm" runat="server" Text="Confirm" OnClick="btnConfirm_Click" />
    <asp:Button ID="btnCancelPopup" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
</asp:Panel> 

<script type="text/javascript">
    function EnsureNumeric(e) {
        if (!e) var e = window.event;
        var key = e.which || e.keyCode;
        var o = document.getElementById("xx");

        if (key == 9) return true; //tab
        if (key == 8 || key == 46) return true; //backspace - delete
        if (key == 35 || key == 36) return true; //home - end
        if (key == 13) {
            if (self.btnSave) $get(btnSave).click(); //submit the form
            return false;
        }
        var src;
        if (e.target) src = e.target;
        else if (e.srcElement) src = e.srcElement;
        if (src.nodeType == 3) src = src.parentNode; //Safari bug
        if (key == 38) { //up
            var v = parseInt(src.value, 10) + 1;
            src.value = v;
        }
        if (key == 40) {//down
            var v = parseInt(src.value, 10) - 1;
            if (v > -1) src.value = v;
        }
        if (key >= 37 && key <= 40) return true; //arrowkeys are okay
        if (key > 47 && key < 58) return true; //0-9
        if (key > 95 && key < 106) return true; //0-9 keypad
        //anything else- invalid
        e.returnValue = false;
        if (e.preventDefault) e.preventDefault();
        return false;
    }

    function Zeroize() {
        var o = document.getElementById("OrderLines");
        var inputs = o.getElementsByTagName("input");
        for (var i = 0; i < inputs.length; i++) {
            var x = inputs[i];
            
            
            x.value = "0";
        }
    }
</script>
</asp:Content>


