$(document).ready(function () {
    //默认加载第一页
    initPager(getTotalData("/SystemApplication/GetRoleCount"), 1);
    //双击打开角色所拥有的权限
    $('.role-box').dblclick(function () {
        var T = $(this);                 //给faq-box定义一个变量
        var state = T.attr('data-state');//给这个变量添加一个状态
        var roleId;
        T.attr('data-state', state == 1 ? "0" : "1");
        if (state == 1) {                    //判断这个状态是否为1
            T.find('.nr').slideToggle();
            T.find('.icon-add').removeClass("hide");
            T.find('.icon-jian').addClass("hide");
            $(".role-box .text-box img[name='spread']").attr("src", "../../images/ulist.png");
        } else {
            T.find('.nr').slideToggle(); //状态为1时，内容显示，加号变减号
            T.find('.icon-add').addClass("hide");
            T.find('.icon-jian').removeClass("hide");
            $(".role-box .text-box img[name='spread'").attr("src", "../../images/uew_icon.png");
        }
    })

});

//currentPage当前页码，PageCount当前页显示数量
function init(PageIndex, PageCount) {
    var roleList = requestUrl("/SystemApplication/GetRole", { 'currentPage': PageIndex, 'pageCount': PageCount });
    roleList = eval(roleList);
    var content = "";
    for (var i = 0; i < roleList.length; i++) {
        content += "<div class='role-box'><div class='text-box'>";
        content += "<span class='title2' tip='" + roleList[i].RDesc + "'>";
        content += "<img alt= '' name='spread' src= '../../images/ulist.png' />角色:";
        content += "<strong style='font-size:16px;'>" + roleList[i].Role + "</strong>";
        content += " ,操作:<img onclick=editDialog('" + roleList[i].RoleId + "','" + roleList[i].Role + "') name='edit' src= '../../images/t02.png' height='16' title='编辑权限'/>";
        content += "|<img onclick=delRole('" + roleList[i].RoleId + "','" + roleList[i].Role + "') name='del' src= '../../images/t03.png' height='16' title='删除角色'/>-<strong>(请谨慎操作,数据无价！)</strong></span>";
        content += "<span style= 'color:gray;'>(双击展开此角色拥有权限)</span> ";
        content += "<div class='icon-box'><span class='icon-add'></span></div></div>";
        var roleauthList = requestUrl("/SystemApplication/GetRoleAuth", null);
        roleauthList = JSON.parse(roleauthList);

        content += "<div class='nr hide'><table style='width:100%;text-align:center;'>";
        for (var j = 0; j < roleauthList.total; j++) {
            if (roleList[i].RoleId == roleauthList.rows[j].ROLEID) {
                content += "<tr><td>" + roleauthList.rows[j].MODULEID + "-" + roleauthList.rows[j].MODULE + "</td></tr>";
            }
        }
        content += "</table></div>";
        $(".hide").append(content);
        content += "</div>";
    }
    $("#roleinfo").append(content);
};

//总记录数 、当前页码
function initPager(RecordCount, PageIndex) {
    $("#page-bottom").setPager({
        RecordCount: RecordCount,
        PageIndex: PageIndex,
        buttonClick: function (RecordCount, PageIndex) {
            $(".role-box").html("");
            initPager(RecordCount, PageIndex);
        }
    });
    //分页同时更新数据源
    init(PageIndex, 15);
};
//弹出角色对话框
function showDialog() {
    //显示内容 
    var content = ''
    content += '<form id=frm><table border= "1" bordercolor= "#0000FF" width= "300" height= "300" cellpadding= "0" cellspacing= "0">'
    content += '<caption><font size="+1">角色信息</font></caption>'
    content += '<tr>'
    content += '<td>角色:</td>'
    content += '<td >'
    content += '<input id="role" name=record.Role type="text" class="comuserinfo">'
    content += '</td>'
    content += '</tr>'
    content += '<tr>'
    content += '<td>描述:</td>'
    content += '<td>'
    content += '<input id="RDesc" name=record.RDesc type="text" class="comuserinfo">'
    content += '</td>'
    content += '</tr>'
    content += '<tr>'
    content += '<td align="center" colspan="2">'
    content += '<input class="btn" type="submit" value="确定" onclick="javascript:addRole()">'
    content += '&nbsp;&nbsp;&nbsp;&nbsp;'
    content += '<input class="btn" type="submit" value="取消" onclick=art.dialog.list["roles"].close(); />'
    content += '</td>'
    content += '</tr>'
    content += '</table></form>'
    //弹出对话框
    art.dialog({
        id: 'roles',
        title: '<center>角色</center>',
        content: content
    });
    $('select[tip],select[reg],input[reg],input[tip],textarea[tip],textarea[reg]').tooltip();
}
//编辑权限
function editDialog(roleId, role) {
    var content = '<div id="editAuthority" border="1" bordercolor="#0000FF" width="500" cellpadding="0" cellspacing="0">'
    content += '<caption> <font size="+1">角色权限</font></caption >'
    content += '<div></div>'
    content += '</div>';

    console.log(content);

    var title = '编辑' + role + '拥有权限';
    //弹出编辑对话框
    art.dialog({
        id: "authority",
        title: title,
        content: content
    });
    //全部权限模块
    $("#editAuthority div").html("");

    console.log($("#editAuthority").html());

    var auth = "";
    //所有权限
    var module_auth = requestUrl("/SystemApplication/GetModule", null);
    module_auth = eval(module_auth);
    //该角色拥有权限
    var authByRoleId = requestUrl("/SystemApplication/GetAuthByRoleId", { 'roleId': roleId });
    authByRoleId = eval("(" + authByRoleId + ")");

    auth += "<fieldset><legend>模块权限列表</legend>";
    //循环所有模块权限
    for (var i = 0; i < module_auth.length; i++) {
        if (module_auth[i].Directory == "/Home/index") {
            auth += "</br><input type='checkbox' value='" + module_auth[i].ModuleId + "'/>" + module_auth[i].Module + ":&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</br>";
        } else { 
        //一行显示五个
        //if ((i + 1) % 5 === 0) {
        //    auth += "<input type='checkbox' value='" + module_auth[i].ModuleId + "'/>" + module_auth[i].Module + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</br>";
        //} else {
            
                auth += "<input type='checkbox' value='" + module_auth[i].ModuleId + "'/>" + module_auth[i].Module + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            //}
        }
    }
    auth += "</fieldset>";
    //确认与取消按钮
    auth += "</br><center><span id='roleId' style='display:none;'>" + roleId + "</span><input class=btn type=submit value=确定 onclick=confirmAuthority()>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input class=btn type=submit value=取消 onclick=art.dialog.list['authority'].close(); /></center>";

    $("#editAuthority div").append(auth);
    //如果拥有权限，则被选中
    for (var j = 0; j < authByRoleId.total; j++) {
        //authByRoleId.rows[j].MODULEID
        $("input[type='checkbox']").each(function () {
            if ($(this).attr("value") == authByRoleId.rows[j].MODULEID) {
                $(this).attr("checked", "true");
            }
        });
    }
    //添加或者取消选中权限[事件]
    $("input[type='checkbox']").click(function () {
        if ($(this)[0].checked == true) {
            $(this).attr('status', 'add');
        }
        else {
            $(this).attr('status', 'del');
        }
    });
}
//确认并添加此角色所拥有权限
function confirmAuthority() {
    var roleId = $("#roleId").text();
    //将添加或者删除的标识传到后台判断作出处理
    var json = "[";
    $("input[status='add']").each(function () {
        json += "{add:'" + $(this)[0].value + "'},";
    });
    $("input[status='del']").each(function () {
        json += "{del:'" + $(this)[0].value + "'},";
    });
    json = json.substring(0, json.length - 1);
    json += "]";
    //请求后台处理
    var result = requestUrl("/SystemApplication/ConfirmAuthority", { "roleId": roleId, "authority": json });
    if (result == "true") {
        art.dialog.list['authority'].close();        
        art.dialog({ time: 5, content: '角色授权成功！', width: 200, height: 125 });        
    }
    else {
        art.dialog.list['authority'].close();
        art.dialog({ time: 5, content: '角色授权失败！', width: 200, height: 125 });

    }
    window.location.href = "/SystemApplication/RoleInfo";
}
//添加角色
function addRole() {
    //var role = $("#role").val();
    //var RDesc = $("#RDesc").val();

    if (role == "") {
        $("#role").addClass("tooltipinputerr");
    }
    else {
        //var result_role = requestUrl("/SystemApplication/AddRole", { 'role': role, 'RDesc': RDesc });
        var result_role = requestUrl("/SystemApplication/AddRole", $("#frm").serialize());
        if (result_role == "true") {
            art.dialog.list['roles'].close();            
            art.dialog({ time: 5, content: '角色添加完成!', width: 200, height: 125 });
        }
        else {
            art.dialog.list['roles'].close();
            art.dialog({ time: 5, content: '角色添加失败!', width: 200, height: 125 });
        }
        window.location.href = "/SystemApplication/RoleInfo";
    }
}
//删除角色
function delRole(roleId, role) {
    if (confirm("确定要删除角色为'" + role + "'的这条记录吗？")) {
        var result_role = requestUrl("/SystemApplication/DeleteRole", { 'roleId': roleId, 'role': role });
        if (result_role == "true") {
            window.location.href = "/SystemApplication/RoleInfo";
            art.dialog({ time: 5, content: '删除成功!', width: 200, height: 125 });
        }
        else {
            art.dialog({ time: 5, content: '删除失败!', width: 200, height: 125 });
        }
        window.location.href = "/SystemApplication/RoleInfo";
    }
}
