
//日期格式化
Date.prototype.Format = function (fmt) { //author: calvin 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
function isVerify(userId) {
    var result = requestUrl("/Shared/IsVerifyAuthority", { "userId": userId })
    return result;
}
//检查功能有效性
function isVaild(node, id) {
    var result = requestUrl("/Shared/GetIsVaild", { "node": node, "id": id });
    return result;
}
//请求url服务
//无data参数时，设置为null 参数: {"a":a}
function requestUrl(url, data) {
    var rnt;
    $.ajax({
        type: "post",
        async: false,
        cache: false,
        data: data,
        url: url,
        success: function (data) {
            rnt = data;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("程序异常!" + url);
        }
    });
    return rnt;
}
//获取总记录数
function getTotalData(url) {
    return requestUrl(url, null);
}

//获取url传递的参数
function getQueryStr(str) {
    var LocString = String(window.document.location.href);
    var rs = new RegExp("(^|)" + str + "=([^\&]*)(\&|$)", "gi").exec(LocString), tmp;
    if (tmp = rs) {
        return tmp[2];
    }
    return "";
}

function getTime(type) {
    var myDates = new Date;
    //var year = myDate.getFullYear();//获取当前年
    //var yue = myDate.getMonth() + 1;//获取当前月
    //var date = myDate.getDate();//获取当前日
    //var h = myDate.getHours();//获取当前小时数(0-23)
    //var m = myDate.getMinutes();//获取当前分钟数(0-59)
    //var s = myDate.getSeconds();//获取当前秒
    var myDate;
    switch (type) {
        case "year":
            myDate = myDates.getFullYear();
            break;
        case "month":
            myDate = myDates.getMonth();
            break;
        case "day":
            myDate = myDates.getDate();
            break;
    }

    return myDate;
}

//循环判断对象页面类型进行属性赋值
function Assignment(y) {
    for (attr in y) {
        //判断对象类型 （页面html类型）
        if (y[attr].type == "label") {
            y[attr].value = $("#" + attr + "").text() == '"undefined"' ? "" : $("#" + attr + "").text();
        }
        if (y[attr].type == "text") {
            y[attr].value = $("#" + attr + "").val() == '"undefined"' ? "" : $("#" + attr + "").val();
        }
        if (y[attr].type == "select") {
            y[attr].value = $("#" + attr + "").val() == '"undefined"' ? "" : $("#" + attr + "").val();
        }
        if (y[attr].type == "radio") {

            y[attr].value = $("input[name='" + attr + "']:checked").val();
        }
    }
    //返回赋值对象
    return y;
}

//js对象转换json字符串
function ToJson(y) {

    var json = "{";
    //循环对象属性
    for (attr in y) {
        var i = y[attr].value == 'undefined' ? "" : y[attr].value;
        //json += attr + ":" + i + ",";
        json += '"' + attr + '"' + ":" + '"' + i + '"' + ",";
    }

    json = json.substring(0, json.lastIndexOf(',')) + "}";

    return json;
}

//json转换js对象
function toObject(json, y) {
    //循环对象属性
    for (attr in y) {
        y[attr].value = json[attr];
    }
    return y;
}

//根据类型将对象值绑定到页面控件上
function bindControl(y) {
    for (attr in y) {
        //判断对象类型 （页面html类型）
        if (y[attr].type == "label") {
            $("#" + attr + "").text(y[attr].value);
        }
        if (y[attr].type == "text" || y[attr].type == "select") {
            $("#" + attr + "").val(y[attr].value);
        }
        if (y[attr].type == "radio") {
            $("input[name='" + attr + "'][value='" + y[attr].value + "']").click();
        }
    }
}

function bindControlByTableName(y) {
    for (attr in y) {
        //判断对象类型 （页面html类型）
        if (y[attr].type == "label") {
            $("[tablename='" + y[attr].tablename + "'][name='" + attr + "']").text(y[attr].value);
        }
        if (y[attr].type == "text" || y[attr].type == "select") {
            $("[tablename='" + y[attr].tablename + "'][name='" + attr + "']").val(y[attr].value);
        }
        if (y[attr].type == "radio") {
            $("[tablename='" + y[attr].tablename + "'][name='" + attr + "'][value='" + y[attr].value + "']").click();
        }
    }
}

function clearNoNum(obj) {
    //先把非数字的都替换掉，除了数字和.
    obj.value = obj.value.replace(/[^\d.]/g, "");
    //必须保证第一个为数字而不是.
    obj.value = obj.value.replace(/^\./g, "");
    //保证只有出现一个.而没有多个.
    obj.value = obj.value.replace(/\.{2,}/g, ".");
    //保证.只出现一次，而不能出现两次以上
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
}

//将c#序列化中的日期通过正则变成字符串
function formatTime(val) {
    var re = /-?\d+/;
    var m = re.exec(val);
    var d = new Date(parseInt(m[0]));
    return d.format("yyyy-MM-dd hh:mm:ss");
}
Date.prototype.format = function (format) //author: meizz
{
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
        (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
            RegExp.$1.length == 1 ? o[k] :
                ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}