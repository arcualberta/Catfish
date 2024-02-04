/**
 * Model for form fields.
 */
/**
 * Field Types
 */
export var FieldType;
(function (FieldType) {
    FieldType[FieldType["ShortAnswer"] = 0] = "ShortAnswer";
    FieldType[FieldType["Paragraph"] = 1] = "Paragraph";
    FieldType[FieldType["RichText"] = 2] = "RichText";
    FieldType[FieldType["Date"] = 3] = "Date";
    FieldType[FieldType["DateTime"] = 4] = "DateTime";
    FieldType[FieldType["Decimal"] = 5] = "Decimal";
    FieldType[FieldType["Integer"] = 6] = "Integer";
    FieldType[FieldType["Email"] = 7] = "Email";
    FieldType[FieldType["Checkboxes"] = 8] = "Checkboxes";
    FieldType[FieldType["DataList"] = 9] = "DataList";
    FieldType[FieldType["RadioButtons"] = 10] = "RadioButtons";
    FieldType[FieldType["DropDown"] = 11] = "DropDown";
    FieldType[FieldType["InfoSection"] = 12] = "InfoSection";
    FieldType[FieldType["AttachmentField"] = 13] = "AttachmentField";
    FieldType[FieldType["CompositeField"] = 14] = "CompositeField";
})(FieldType || (FieldType = {}));
/**
 * Text input field types, including types applicable for multi-lingual text fields.
 * */
export var TextType;
(function (TextType) {
    TextType[TextType["ShortAnswer"] = 0] = "ShortAnswer";
    TextType[TextType["Paragraph"] = 1] = "Paragraph";
    TextType[TextType["RichText"] = 2] = "RichText";
    TextType[TextType["AttachmentField"] = 13] = "AttachmentField";
})(TextType || (TextType = {}));
/**
 * Mono-lingual text field types.
 * */
export var MonolingualFieldType;
(function (MonolingualFieldType) {
    MonolingualFieldType[MonolingualFieldType["Date"] = 3] = "Date";
    MonolingualFieldType[MonolingualFieldType["DateTime"] = 4] = "DateTime";
    MonolingualFieldType[MonolingualFieldType["Decimal"] = 5] = "Decimal";
    MonolingualFieldType[MonolingualFieldType["Integer"] = 6] = "Integer";
    MonolingualFieldType[MonolingualFieldType["Email"] = 7] = "Email";
})(MonolingualFieldType || (MonolingualFieldType = {}));
/**
 * Option field types.
 * */
export var OptionFieldType;
(function (OptionFieldType) {
    OptionFieldType[OptionFieldType["Checkboxes"] = 8] = "Checkboxes";
    OptionFieldType[OptionFieldType["DataList"] = 9] = "DataList";
    OptionFieldType[OptionFieldType["RadioButtons"] = 10] = "RadioButtons";
    OptionFieldType[OptionFieldType["DropDown"] = 11] = "DropDown";
})(OptionFieldType || (OptionFieldType = {}));
/**
 * Field types that does not take any user input.
 * */
export var InfoSectionType;
(function (InfoSectionType) {
    InfoSectionType[InfoSectionType["InfoSection"] = 12] = "InfoSection";
})(InfoSectionType || (InfoSectionType = {}));
export var CompositeFieldType;
(function (CompositeFieldType) {
    CompositeFieldType[CompositeFieldType["CompositeField"] = 14] = "CompositeField";
})(CompositeFieldType || (CompositeFieldType = {}));
//# sourceMappingURL=field.js.map