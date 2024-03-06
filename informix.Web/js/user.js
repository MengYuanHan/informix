$(document).ready(function () {
    //默认加载第一页
    initPager(getTotalData("/SystemApplication/GetUserCount"), 1);
    //判断启用/停止用户功能
    if (isVaild("authority", "startup") == "true" && isVaild("authority", "blockup") == "true") {
        $(".tablelist tbody tr td img").removeClass("isvaild");
    }
});

//currentPage当前页码，PageCount当前页显示数量
function init(PageIndex, PageCount) {
    var userList = requestUrl("/SystemApplication/GetUser", { 'currentPage': PageIndex, 'pageCount': PageCount });
    userList = eval(userList);

    var content = "";
    for (var i = 0; i < userList.length; i++) {
        var n = null;
        var imgUrl = null;
        var title = null;
        var userId = userList[i].UserId;
        if (userList[i].Sex == "1") {
            n = "男";
        }
        else {
            n = "女";
        }
        if (userList[i].State == "0") {
            imgUrl = "../images/on.png";
            title = "启用";
        }
        else {
            imgUrl = "../images/off.png";
            title = "停用";
        }
        content += '<tr><td><a class="a" title="修改用户信息" onclick="verifyAuthority(' + userId + ')" >' + userList[i].UserName + '</a></td>';
        content += '<td>' + n + '</td>';
        content += '<td>' + userList[i].RealName + '</td>';
        content += '<td>' + userList[i].Province + '</td>';
        content += '<td>' + userList[i].City + '</td>';
        content += '<td>' + userList[i].Address + '</td>';
        content += '<td>' + userList[i].Phone + '</td>';
        content += '<td>' + userList[i].Fax + '</td>';
        content += '<td>' + userList[i].Email + '</td>';
        content += '<td>' + userList[i].QQ + '</td>';
        content += '<td>' + userList[i].NickName + '</td>';
        content += '<td><a href="#" onclick="changeImg(' + userId + ',' + userList[i].State + ')"><img id="' + userId + '" src="' + imgUrl + '" height="30px" width="30px" title="' + title + '"/></a></td></tr>';
    }

    $(".tablelist tbody").append(content);
};

//总记录数 、当前页码
function initPager(RecordCount, PageIndex) {
    $("#page-bottom").setPager({
        RecordCount: RecordCount,
        PageIndex: PageIndex,
        buttonClick: function (RecordCount, PageIndex) {
            $(".tablelist tbody").html("");
            initPager(RecordCount, PageIndex);
        }
    });
    //分页同时更新数据源
    init(PageIndex, 15);
};

//切换按钮图片
function changeImg(userId, state) {
    var s = state == "0" ? "1" : "0";
    var result = requestUrl("/SystemApplication/UpdateState", { "userId": userId, "state": s });

    //$("#" + userId + "").attr("src", "");
    if (result == "0") {
        swal('Success', '用户已启用', 'success');
        $("#" + userId + "").attr("src", "../images/on.png");
    }
    else if (result == "1") {
        swal('Success', '用户已停用', 'success');
        $("#" + userId + "").attr("src", "../images/off.png");
    }
    else {
        swal('Warning', '当前用户没有权限', 'warning');
    }
    $(".tablelist tbody").html("");
    initPager(getTotalData("/SystemApplication/GetUserCount"), 1);
    return s;
}

//验证用户是都有修改权限
function verifyAuthority(userId) {
    //判断当前登录用户是否有权限修改
    var _userId = requestUrl("/Shared/CurrentUserId", null);
    if (isVaild("authority", "updateuser") == "true" && isVerify(_userId) == "true") {
        window.location.href = "/SystemApplication/UserUpdate?UserId=" + userId;
    }
    else {
        //如果当前登录用户UserId与此用户记录的userId值一样，则可以修改
        if (_userId == userId) {
            window.location.href = "/SystemApplication/UserUpdate?UserId=" + userId;
        }
        else {
            swal('Warning', '没有权限，只能修改当前登录用户', 'warning');
        }
    }
}

//添加用户
function addUser() {
    var userName = $("#username").val();
    var password = $("#password").val();
    var realname = $("#realname").val();
    var sex = $("input[name='sex']:checked").val();
    var title = $("#title").val();
    var phone = $("#phone").val();
    var fax = $("#fax").val();
    var email = $("#email").val();
    var province = $("#province option:selected").val();
    var city = $("#city option:selected").val();
    var address = $("#address").val();
    var QQ = $("#QQ").val();
    var nickname = $("#nickname").val();
    var dept = $("#dept option:selected").val();
    var role = $("#role option:selected").val();
    if (userName == "" || password == "" || province == "" || city == "" || realname == "" || address == "" || dept == "" || role == "") {
        $("#username,#password,#province,#city,#realname,#address,#dept,#role").addClass("tooltipinputerr");
    }
    else {
        //var result_user = requestUrl("/SystemApplication/AddUser", { "UserName": userName, "Password": password, "Sex": sex, "RealName": realname, "Title": title, "Phone": phone, "Fax": fax, "Email": email, "Province": province, "City": city, "Address": address, "QQ": QQ, "NickName": nickname, "DeptId": dept, "RoleId": role });
        var result_user = requestUrl("/SystemApplication/AddUser", $("#frm").serializeArray());
        if (result_user == "true") {
            art.dialog.list['userinfo'].close();
            swal('Success', '用户添加成功', 'success');
        }
        else {
            art.dialog.list['userinfo'].close();
            swal('Fail', '用户添加失败', 'error');
        }
    }
}
//弹出添加用户对话框
function showDialog() {
    //添加部门信息
    var result_dept = requestUrl("/SystemApplication/GetDepartment", null);
    result_dept = eval(result_dept);
    var dept = "";
    for (var i = 0; i < result_dept.length; i++) {
        dept += "<option value='" + result_dept[i].DeptId + "'>" + result_dept[i].Dept + "</option>";
    }
    $("#dept").append(dept);

    //添加部门信息
    var result_Roles = requestUrl("/SystemApplication/GetRoles", null);
    result_Roles = eval(result_Roles);
    var dept = "";
    for (var i = 0; i < result_Roles.length; i++) {
        dept += "<option value='" + result_Roles[i].RoleId + "'>" + result_Roles[i].Role + "</option>";
    }
    $("#role").append(dept);

    //显示内容
    var content = $("#userinfo").html();
    //弹出对话框
    art.dialog({
        id: 'userinfo',
        title: '用户资料',
        content: content,
        width: 525
    });
    $('select[tip],select[reg],input[reg],input[tip],textarea[tip],textarea[reg]').tooltip();
}

