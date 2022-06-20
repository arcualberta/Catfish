export enum eRefType { undefined, data, metadata }

export enum eValidationStatus {
    VALID = 'VALID',
    VALUE_REQUIRED = 'VALUE_REQUIRED',
    INVALID = 'INVALID'
}

export enum eFieldType {
    AttachmentField,
    AudioRecorderField,
    CheckboxField,
    CompositeField,
    DateField,
    DecimalField,
    EmailField,
    FieldContainerReference,
    InfoSection,
    IntegerField,
    MonolingualTextField,
    RadioField,
    SelectField,
    TableField,
    TextArea,
    TextField,
}

export enum eDataElementType {
    Text,
    Option,
    FileReference
}