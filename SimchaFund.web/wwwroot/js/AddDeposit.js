$(() => {
    console.log('test')
    $(".deposit-button").on('click', function () {
        let name = $(this).data("contributorname");
        $("#deposit-name").append(name);
        console.log($(this).data("contributorid"));
        let id = $(this).data("contributorid");
        console.log(id)
        $("#hiddenId").val(id);
        $("#modal-deposit").modal()
    })
})