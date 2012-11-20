<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Web.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div style="margin-top: 10px; margin-bottom: 10px">
<asp:Label ID="lblFeedBack" runat="server" />
</div>

<asp:Panel ID="pnlListOrders" runat="server">
    <asp:Repeater  ID="repeatOrders" runat="server"
            onitemcommand="repeatOrders_ItemCommand">
            <HeaderTemplate>
                <table class="ListOrders">
                    <thead>
                        <tr>
                            <th>Action</th>
                            <th>Order id</th>
                            <th>Company name</th>
                            <th>Attention name</th>
                            <th>AddressLine1</th>
                            <th>City</th>
                            <th>Postal code</th>
                            <th>CC</th>
                            <th>Status</th>
                            <th>OrderLine id</th>
                            <th>Part id</th>
                            <th>Part name</th>
                            <th>Order qty</th>
                            
                        </tr>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <asp:Button Visible='<%# (int)Eval("Status") == (int)ShippingService.Business.Domain.OrderStatus.Unpacked %>' ID="btnPack" runat="server" CommandName="PACK" CommandArgument='<%# Eval("Id") %>' Text="Pack" />
                        <asp:Button Visible='<%# (int)Eval("Status") == (int)ShippingService.Business.Domain.OrderStatus.Packed %>' ID="btnShip" runat="server" CommandName="SHIP" CommandArgument='<%# Eval("Id") %>' Text="Ship" />
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("Id") %>
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("MainAddress.CompanyName")%>
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("MainAddress.AttentionName")%>
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("MainAddress.AddressLine1")%>
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("MainAddress.City")%>
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("MainAddress.PostalCode")%>
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("MainAddress.CountryCode")%>
                    </td>
                    <td rowspan='<%# Eval("Lines.Count") %>'>
                        <%# Eval("Status")%>
                    </td>
                    
                    
                     <% isFirstRow = true;%>
                    <asp:Repeater ID="repeaterOrderLines" runat="server" DataSource='<%# Eval("Lines") %>'>
                        <ItemTemplate>
                            <%if (!isFirstRow){%><tr><%}%>
                                    <td><%# Eval("Id") %></td>
                                    <td><%# Eval("PartId") %></td>
                                    <td><%# Eval("PartName") %></td>
                                    <td><%# Eval("OrderQty") %></td>
                                </tr>
                                <%isFirstRow = false; %>
                        </ItemTemplate>    
                    </asp:Repeater>
                    
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
  </asp:Panel>
</asp:Content>
