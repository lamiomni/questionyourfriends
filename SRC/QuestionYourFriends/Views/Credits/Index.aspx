
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>
    <h2> Facebook Credits Demo Application</h2>

  <p> Create an order by specifying the following attributes:</br>
  <i>Title, price, description, image URL and product URL</i></p>
  
  <!-- Please note that user can change any information in order_info through 
	javascript. So please make sure you never put price or any other 
	information you don't want users to modify in order_info. We put everything
	here only for end-to-end flow testing purpose!! -->
  <form name ="place_order" id="order_form" action="#">
  Title:       <input type="text" name="title" value="BFF Locket"
                id="title_el"> </br></br>
  Price:       <input type="text" name="price" value="10"
                id="price_el"> </br></br>
  Description: <input type="text" name="description" size="64"
                value="This is a BFF Locket..." id="desc_el"> </br></br>
  Image URL:   <input type="text" name="image_url" size="64"
                value="http://www.facebook.com/images/gifts/21.png"
                id="img_el"> </br></br>
  Product URL: <input type="text" name="product_url" size="64"
                value="http://www.facebook.com/images/gifts/21.png"
                id="product_el"> </br></br>
  <a onclick="placeOrder(); return false;">
    <img src="http://www.facebook.com/connect/button.php?app_id=131910193550231&feature=payments&type=light_l">
  </a>
  </form>


  
  <div id="output">  </div> </br></br>
    <script type="text/javascript">
       
    function placeOrder() {
      var title = document.getElementById('title_el').value;
      var desc = document.getElementById('desc_el').value;
      var price = document.getElementById('price_el').value;
      var img_url = document.getElementById('img_el').value;
      var product_url = document.getElementById('product_el').value;

      var order_info = 'abc123';

      // calling the API ...
      var obj = {
        method: 'pay',
        order_info: order_info,
        purchase_type: 'item'
    };

      FB.ui(obj, callback);
    }
   

    var callback = function (data) {
        if (data['order_id']) {
            writeback("Transaction Completed! </br></br>"
        + "Data returned from Facebook: </br>"
        + "<b>Order ID: </b>" + data['order_id'] + "</br>"
        + "<b>Status: </b>" + data['status']);
        } else if (data['error_code']) {
            writeback("Transaction Failed! </br></br>"
        + "Error message returned from Facebook:</br>"
        + data['error_message']);
        } else {
            writeback("Transaction failed!");
        }
    };

    function writeback(str) {
      document.getElementById('output').innerHTML=str;
    }

  </script>
</asp:Content>
