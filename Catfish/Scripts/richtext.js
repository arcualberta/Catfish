function convertToRichTextSwapper(selector, editorCss, width = "100%", height = "340", autoresizeMinHeight = 340, preventCreationOfHtmlCharacters = false, plainTextLabel = "Plain Text", richTextLabel = "Rich Text") {
    var initData = {
        mode: 'specific_textareas',
        selector: selector,
        convert_urls: false,
        plugins: [
            "autoresize autolink anchor code hr paste piranhaimage link wordcount lists searchreplace textcolor visualblocks"
        ],
        width: width,
        height: height,
        autoresize_min_height: autoresizeMinHeight,
        autoresize_max_height: height,
        toolbar: "bold italic underline | bullist numlist hr | formatselect removeformat | cut copy paste | link piranhaimage | superscript subscript | indent outdent | code",
    };

    if (editorCss && editorCss != null) {
        initData.content_css = editorCss;
    }

    if (preventCreationOfHtmlCharacters) {
        initData["forced_root_block"] = false;
        initData["force_br_newlines"] = false;
        initData["force_p_newlines"] = false;
        initData["convert_newlines_to_brs"] = false;
    }

    tinymce.init(initData);
}