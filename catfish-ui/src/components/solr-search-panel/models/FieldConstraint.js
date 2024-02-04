import { eConstraintType, eFieldConstraint } from "@/components/shared/constants";
export const createFieldConstraint = () => {
    return {
        field: null,
        constraint: eFieldConstraint.Equals,
        value: null,
        type: eConstraintType.FieldConstraint
    };
};
//# sourceMappingURL=FieldConstraint.js.map