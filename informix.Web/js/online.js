$(document).ready(function () {
    //默认加载第一页
    initPager(getTotalData("/SystemApplication/GetOnlineCount"), 1);
});

//currentPage当前页码，PageCount当前页显示数量
function init(PageIndex, PageCount) {
    var onlineList = requestUrl("/SystemApplication/GetOnline", { 'currentPage': PageIndex, 'pageCount': PageCount });
    onlineList = eval(onlineList);
    var content = "";
    for (var i = 0; i < onlineList.length; i++) {
        var time = null;
        time = formatTime(onlineList[i].LoginTime);
        content += "<tr>";
        content += "<td>" + onlineList[i].UserId + "</td>";
        content += "<td>" + onlineList[i].UserName + "</td>";
        content += "<td>" + onlineList[i].GUID + "</td>";
        content += "<td>" + onlineList[i].IP + "</td>";
        content += "<td>" + time + "</td>"; 
        content += "<td>" + onlineList[i].isVailable + "</td>";
        content += "<td><a class='tablelink'  onclick=offline('" + onlineList[i].GUID + "') href='#'>下线</a></td>";
    }
    $(".tablelist").append(content);
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
function offline(guid) {
    var result = requestUrl("/SystemApplication/Offline", { "guid": guid});
    if (result == "1") {
        //是否强制下线
        if (confirm("是否强制下线？")) {
            window.open('/account/login', '_parent');
        }        
    }
    else {
        art.dialog({ time: 3, content: '下线成功!', width: 200, height: 125 });
    }
}
