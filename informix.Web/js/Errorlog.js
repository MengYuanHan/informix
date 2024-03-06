$(document).ready(function () {
    init();
});
function init() {
    var result_log = requestUrl("/SystemMaintenance/GetFileInfo", null)
    if (result_log != "]") {
        result_log = eval("[" + result_log + "]");
        var content = "";
        for (var i = 0; i < result_log.length; i++) {
            content += '<tr>';
            content += '<td>' + result_log[i].FileName + '</td>';
            content += '<td>' + result_log[i].FileType + '</td>';
            content += '<td>' + result_log[i].FileDir + '</td>';
            content += '<td>' + result_log[i].FileLen + '</td>';
            content += '<td>' + result_log[i].CreateTime + '</td>';
            content += '<td>' + result_log[i].LaseTime + '</td>';
            content += '<td>' + result_log[i].AccessTime + '</td>';
            content += '<td><a class="tablelink" onclick=javascript:read("' + result_log[i].FileDir + '")>查看</a>||<a class="tablelink" onclick=javascript:deleteLog("' + result_log[i].FileDir + '")>删除</a></td>';
            content += '</tr>';
        }
        $(".tablelist tbody").append(content);
    }
}
function read(dir) {
    var result = requestUrl("/SystemMaintenance/OpenLog", { 'dir': dir });
}
function deleteLog(dir) {
    var result = requestUrl("/SystemMaintenance/DeleteLog", { 'dir': dir });
    art.dialog({ time: 2, content: result, width: 200, height: 125 });
    window.location.href = "/SystemApplication/EventLog";
}