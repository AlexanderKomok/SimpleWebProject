
$(document).ready(function () {
    
    DeleteFromAlbumAndReload();
    AddToAlbumAndReload();   
});

function DeleteFromAlbumAndReload() {
    //Delete from album and reload
    $('.trackAlbumItem').off('click');
    $('.trackAlbumItem').on('click', function () {
        
        var TrackId = $(this).find('input').attr("value3");
        let params = new URLSearchParams(location.search);
        var AlbumId = params.get('AlbumId');
        
        debugger;
        $.ajax({
            type: "POST",
            url: "/Album/OutAlbum",
            data: { AlbumId: AlbumId, TrackId: TrackId },
            async: false,
            success: function () {
                $.ajax({
                    type: "POST",
                    url: "/Album/DisplayAllAlbum",
                    data: { AlbumId: AlbumId },
                    async: false,
                    success: function (data) {
                        console.log(data);
                        $('#All').html(data);
                        DeleteFromAlbumAndReload();

                    },
                    error: function (data) {
                        console.log(data);
                    }
                })
            }
        })
    })
}

function AddToAlbumAndReload() {

    //Add to Album and reload
    $('.trackItem').off('click');
    $('.trackItem').on('click', function () {
        //var AlbumId = $("#AlbIdVal").val();
        var TrackId = $(this).find('input').attr("value1");
        //var AlbumId = $(this).find('input').attr("value2");
        let params = new URLSearchParams(location.search);
        var AlbumId = params.get('AlbumId');

        $.ajax({
            type: "POST",
            url: "/Album/InAlbum",
            data: { AlbumId: AlbumId, TrackId: TrackId },
            async: false,
            success: function () {
                //console.log("DataInController")
                $.ajax({
                    type: "POST",
                    url: "/Album/DisplayAllAlbum",
                    data: { AlbumId: AlbumId },
                    async: false,
                    success: function (data) {
                        $('#All').html(data);
                        //setTimeout(function () { }, 1000);
                        AddToAlbumAndReload();
                    }
                })
            },

            error: function (data) {
                console.log(data);
            }

        })
    })
}