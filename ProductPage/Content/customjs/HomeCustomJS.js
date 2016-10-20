function loadProductByShopAndCategory(shopId, categoryId, start, quantity, categoryName, loadMore) {
    if (loadMore == 0) {
        //Set Category Name
        $('#divCateName').empty();
        $('#divCateName').append("<h2>" + categoryName + "</h2><hr style='border: 0.1px solid'>");
        $('#productContent').empty();
        $('#divLoadMore').empty();
    }
    var imgDiv = null;
    var imgUrl = null;
    var saleDiv = null;
    var divPrice = null;
    var divPromotionPrice = null;
    $.ajax({
        url: "/GetData/GetProductByShopAndCategory",
        data: {
            shopId: shopId,
            categoryId: categoryId,
            start: start,
            quantity: quantity
        },
        dataType: "json",
        success: function (data) {
            if (data.length != 0) {
                for (var i = 0; i < data.length; i++) {
                    imgDiv = '';
                    imgUrl = '';
                    saleDiv = '';
                    divPrice = '';
                    divPromotionPrice = '';
                    if (data[i].Urls.length != 0) {
                        imgUrl = data[i].Urls[0];
                        imgDiv = "<div class='product-imitation' style='height:198.29px;background: url(\"" + imgUrl + "\") center no-repeat;background-size: contain'></div>"
                    } else {
                        imgDiv = "<div class='product-imitation'>Chưa có hình ảnh</div>";
                    }
                    var priceToSale = null;
                    if (data[i].PromotionPrice != null && data[i].PromotionPrice < data[i].Price) {
                        divPrice = "<s class='text-muted'>" + data[i].Price + "</s>";
                        divPromotionPrice = "<h4 style='color:red;font-size:20px; display: inline'>" + data[i].PromotionPrice + "</h4>"
                        saleDiv = "<span class='product-price' style='background-color:orangered'>Sales</span>";
                        priceToSale = data[i].PromotionPrice;
                    }
                    if (data[i].PromotionPrice == null) {
                        divPrice = "<h4 style='font-size:20px; display: inline'>" + data[i].Price + "</h4>";
                        priceToSale = data[i].Price;
                    }
                    $('#productContent').append("<div class='col-sm-4'>"
                        + "<div class='ibox'>"
                        + "<div class='ibox-content product-box' style='max-width:100%; max-height:100%'>"
                                    + imgDiv
                                    + " <div class='product-desc'>"
                                        + saleDiv
                                        + "<a href='javascript:void(0)' class='product-name' onclick='viewDetail(" + shopId + "," + categoryId + "," + data[i].Id + ")'> " + data[i].Name + "</a>"
                                        + "<div class='small m-t-xs'>"
                                            + divPrice
                                            + "&nbsp;"
                                            + divPromotionPrice
                                        + "</div>"
                                        + "<div class='m-t text-righ'> <a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + data[i].Id + ",\"" + data[i].Name + "\",\"" + imgUrl + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a> </div>"
                                    + "</div>"
                                + "</div>"
                            + "</div>"
                        + "</div>");
                }
                if (data.length >= quantity) {
                    $('#divLoadMore').empty();
                    $('#divLoadMore').append("<button class='btn btn-primary' onclick='loadProductByShopAndCategory(" + shopId + "," + categoryId + "," + parseInt(start + quantity) + "," + quantity + ",1)'>Thêm sản phẩm</button>");
                } else {
                    if (data.length < quantity) {
                        $('#divLoadMore').empty();
                    }
                };
            }
        },
        error: function () {
            $('#productContent').append("<h2 class='text-center'>Chưa có sản phẩm</h2>");
        }
    })
}


function loadProductOfParentCategory(shopId, parentId, start, quantity, categoryName, loadMore) {
    if (loadMore == 0) {
        //Set Category Name
        $('#divCateName').empty();
        $('#divCateName').append("<h2>" + categoryName + "</h2><hr style='border: 0.1px solid'>");
        $('#productContent').empty();
        $('#divLoadMore').empty();
    }
    var imgDiv = null;
    var imgUrl = null;
    var saleDiv = null;
    var divPrice = null;
    var divPromotionPrice = null;
    $.ajax({
        url: "/GetData/GetAllProductOfChildCategory",
        data: {
            shopId: shopId,
            parentCategoryId: parentId,
            start: start,
            quantity: quantity
        },
        dataType: "json",
        success: function (data) {
            if (data.length != 0) {
                for (var i = 0; i < data.length; i++) {
                    imgDiv = '';
                    imgUrl = '';
                    saleDiv = '';
                    divPrice = '';
                    divPromotionPrice = '';
                    if (data[i].Urls.length != 0) {
                        imgUrl = data[i].Urls[0];
                        imgDiv = "<div class='product-imitation' style='height:198.29px;background: url(\"" + imgUrl + "\") center no-repeat;background-size: contain'></div>"
                    } else {
                        imgDiv = "<div class='product-imitation'>Chưa có hình ảnh</div>";
                    }
                    var priceToSale = null;
                    if (data[i].PromotionPrice != null && data[i].PromotionPrice < data[i].Price) {
                        divPrice = "<s class='text-muted'>" + data[i].Price + "</s>";
                        divPromotionPrice = "<h4 style='color:red;font-size:20px; display: inline'>" + data[i].PromotionPrice + "</h4>"
                        saleDiv = "<span class='product-price' style='background-color:orangered'>Sales</span>";
                        priceToSale = data[i].PromotionPrice;
                    }
                    if (data[i].PromotionPrice == null) {
                        divPrice = "<h4 style='font-size:20px; display: inline'>" + data[i].Price + "</h4>";
                        priceToSale = data[i].Price;
                    }
                    $('#productContent').append("<div class='col-sm-4'>"
                        + "<div class='ibox'>"
                        + "<div class='ibox-content product-box' style='max-width:100%; max-height:100%'>"
                                    + imgDiv
                                    + " <div class='product-desc'>"
                                        + saleDiv
                                        + "<a href='javascript:void(0)' class='product-name' onclick='viewDetail(" + shopId + "," + data[i].CategoryId + "," + data[i].Id + ")'> " + data[i].Name + "</a>"
                                        + "<div class='small m-t-xs'>"
                                            + divPrice
                                            + "&nbsp;"
                                            + divPromotionPrice
                                        + "</div>"
                                        + "<div class='m-t text-righ'> <a href='javascript:void(0)' class='btn btn-xs btn-outline btn-primary' onclick='addToCart(" + data[i].Id + ",\"" + data[i].Name + "\",\"" + imgUrl + "\"," + priceToSale + ",1)'><i class='fa fa-shopping-cart'> Thêm vào giỏ hàng</i> </a> </div>"
                                    + "</div>"
                                + "</div>"
                            + "</div>"
                        + "</div>");
                }
                if (data.length == quantity) {
                    $('#divLoadMore').empty();
                    $('#divLoadMore').append("<button class='btn btn-primary' onclick='loadProductOfParentCategory(" + shopId + "," + parentId + "," + parseInt(start + quantity) + "," + quantity + "," + '"' + categoryName + '"' + ",1)'>Thêm sản phẩm</button>");
                } else {
                    if (data.length < quantity) {
                        $('#divLoadMore').empty();
                    }
                };
            } else {
                $('#productContent').append("<h2 class='text-center'>Chưa có sản phẩm</h2>");
            }
        },
        error: function () {
            $('#productContent').append("<h2 class='text-center'>Chưa có sản phẩm</h2>");
        }
    })
}


function viewDetail(shopId, categoryId, productId) {

    $('#productName').empty();
    $('#productPriceDiv').empty();
    $('#tblAttribute').empty();
    $('#productDescription').empty();
    $("#addToCartOnViewDetail").removeAttr("onclick");
    $.ajax({
        url: "/GetData/GetProductByProductId",
        data: {
            shopId: shopId,
            categoryId: categoryId,
            productId: productId
        },
        dataType: "json",
        success: function (data) {
            $('#divProductImage').slick('slickRemove', null, null, true);
            if (data.Urls.length != 0) {
                for (var i = 0; i < data.Urls.length; i++) {
                    $('#divProductImage').slick('slickAdd', "<div class='product-imitation' style='height:420px;background: url(\"" + data.Urls[i] + "\") center no-repeat;background-size: contain'></div>");
                };
            } else {
                $('#divProductImage').slick('slickAdd', "<div>[Không có hình ảnh]</div>");
            }

            $('#productName').append(data.Name);
            if (data.PromotionPrice == null) {
                $('#productPriceDiv').append("<h2 class='product-main-price inline' id='mainPrice'>" + data.Price + " VND</h2>");
                $('#viewDetail').append("<input id='priceViewDetail' type='hidden' value='" + data.Price + "' />");
                var currentPrice = data.Price;
                $('#quantity').val(1);
            } else {
                if (data.Price != null && data.PromotionPrice <= data.Price) {
                    $('#productPriceDiv').append("<s class='text-muted' id='mainPrice'>" + data.Price + "</s><h2 class='product-main-price inline' style='color:red' id='promotionPrice'>" + data.PromotionPrice + " VND</h2>");
                    $('#viewDetail').append("<input id='priceViewDetail' type='hidden' value='" + data.PromotionPrice + "' />");
                    var currentPrice = data.PromotionPrice;
                    $('#quantity').val(1);
                }
            }
            if (!data.IsInStock) {
                $('#productPriceDiv').append("<h3 class='inline' style='color: red'>(Hết hàng)</h3>");
            }
            if (data.Description != null) {
                $('#productDescription').append(data.Description);
            } else {
                $('#productDescription').append("Chưa có mô tả cho sản phẩm này.");
            }
            $('#viewDetail').append("<input id='productIdViewDetail' type='hidden' value='" + productId + "' />");
            $('#viewDetail').append("<input id='productFirstUrl' type='hidden' value='" + data.Urls[0] + "' />");
            $("#addToCartOnViewDetail").attr("onclick", "addToCart(" + data.productId + "," + data.productName + "," + data.Urls[0] + ","+ +"," + $('#quantity').val() + ")");
            if (data.TemplateId != null) {
                $.ajax({
                    url: "/GetData/GetTemplateProductByShopAndId",
                    data: {
                        id: data.TemplateId,
                        shopId: shopId
                    },
                    dataType: "json",
                    success: function (result) {
                        $('#tblAttribute').append("<col width='30%'><col width='70%'>");
                        if (result.Attr1 != null && result.Attr1.length != 0) {
                            if (data.Attr1 == null) {
                                data.Attr1 == '';
                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr1 + "</strong></td><td>" + data.Attr1 + "</td></tr>");
                        };
                        if (result.Attr2 != null && result.Attr2.length != 0) {
                            if (data.Attr2 == null) {
                                data.Attr2 == '';
                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr2 + "</strong></td><td>" + data.Attr2 + "</td></tr>");
                        };
                        if (result.Attr3 != null && result.Attr3.length != 0) {
                            if (data.Attr3 == null) {
                                data.Attr3 == '';
                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr3 + "</strong></td><td>" + data.Attr3 + "</td></tr>");
                        };
                        if (result.Attr4 != null && result.Attr4.length != 0) {
                            if (data.Attr4 == null) {
                                data.Attr4 == '';
                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr4 + "</strong></td><td>" + data.Attr4 + "</td></tr>");
                        };
                        if (result.Attr5 != null && result.Attr5.length != 0) {
                            if (data.Attr5 == null) {
                                data.Attr5 == '';
                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr5 + "</strong></td><td>" + data.Attr5 + "</td></tr>");
                        };
                        if (result.Attr6 != null && result.Attr6.length != 0) {
                            if (data.Attr6 == null) {
                                data.Attr6 == '';
                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr6 + "</strong></td><td>" + data.Attr6 + "</td></tr>");
                        };
                        if (result.Attr7 != null && result.Attr7.length != 0) {
                            if (data.Attr7 == null) {
                                data.Attr7 == '';
                            }
                            $('#tblAttribute').append("<tr><td><strong>" + result.Attr7 + "</strong></td><td>" + data.Attr7 + "</td></tr>");
                        };
                    },
                    error: function () {
                        $('#tblAttribute').append("Chưa có mô tả đặc tính cho sản phẩm này");
                    }
                })
            } else {
                $('#tblAttribute').append("Chưa có mô tả đặc tính cho sản phẩm này");
            }
            $('#viewDetailModal').modal('show');
        },
        error: function () {
            alert("That bai");
        }
    });
}

function addToCart(productId, productName, url, price, quantity) {
    $.ajax({
        url: "/GetData/AddToCart",
        data: {
            productId: productId,
            productName: productName,
            url: url,
            price: price,
            quantity: quantity,
        },
        dataType: "text",
        success: function (data) {
            $('#cartQuantity').text(data);
            toastr.success("Sản phẩm đã được thêm vào giỏ hàng", "Thành công");
        },
        error: function () {
            toastr.error("Sản phẩm chưa được thêm vào giỏ hàng", "Thất bại");
        }
    })
}

function getCart() {
    $.ajax({
        url: "/GetData/GetCart",
        dataType: "json",
        success: function (data) {
            alert(data);
            return data;
        },
        error: function () {
            return null;
        }
    })

}

$("#addToCartOnViewDetail").click(function () {
    var productId = $('#productIdViewDetail').val();
    var price = $('#priceViewDetail').val();
    var quantity = $('#quantity').val();
    var productName = $('#productName').text();
    var productFirstUrl = $('#productFirstUrl').val();
    alert(productId + productName +  price + quantity + productFirstUrl);
    addToCart(productId, productName, productFirstUrl, price, quantity);
});

$("#cart").click(function () {
    $('#viewDetailDiv').empty();
    $.ajax({
        url: "/GetData/GetCart",
        dataType: "json",
        success: function (cart) {
            if (cart != null && cart.length != 0) {
                for (var i = 0; i < cart.length; i++) {
                    var viewDetailItem = $('#viewDetailItem').clone();
                    viewDetailItem.find("a.text-navy").text(cart[i].productName);
                    $('#viewDetailDiv').append(viewDetailDiv);
                }

            } else {
                if (cart.length != 0) {
                    $('#viewDetailDiv').append("<h2 class='text-center'>Chưa có sản phẩm nào trong giỏ hàng</h2>")
                }
            }
        },
        error: function () {
            $('#viewDetailDiv').append("<h2 class='text-center'>Chưa có sản phẩm nào trong giỏ hàng</h2>")
        }
    })
    $('#cartModal').modal('show');
});