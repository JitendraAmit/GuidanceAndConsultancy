/**
 * @license Copyright (c) 2003-2020, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    
    config.toolbar = [
        //{ name: 'document', items: ['Source', '-', 'Save', 'NewPage', 'ExportPdf', 'Preview', 'Print', '-', 'Templates'] },
        //{ name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
        //{ name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll', '-', 'Scayt'] },
        //{ name: 'forms', items: ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'] },
        //'/',
        { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'CopyFormatting', 'RemoveFormat'] },
        { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl', 'Language'] },
        { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
       /* { name: 'insert', items: ['Image','Youtube', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe' ] },*/
        '/',
        { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
        //{ name: 'colors', items: ['TextColor', 'BGColor'] },
        //{ name: 'tools', items: ['Maximize', 'ShowBlocks'] },
        //{ name: 'about', items: ['About'] },
       // { name: 'youtube', items: ['Youtube'] },
        //{ name: 'embed', items: ['Embed'] },
        //{ name: 'embedbase', items: ['Embedbase'] }
    ];

    config.extraPlugins = 'youtube', 'embed', 'embedbase';
    config.font_names = 'Hindi/Kruti;' + config.font_names;
    

};
    
//config.extraPlugins = 'youtube';
 //config.extraPlugins = 'embed';
    //config.toolbarGroups = [
    //    {name:'youtube'}
    //        ]

    
   

