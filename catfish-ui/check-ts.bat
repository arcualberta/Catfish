echo. & vue-tsc --noEmit | findstr /V TS2786 | findstr /V /c:"is not a valid JSX element."  | findstr /V /c:"Property 'model' does not exist on type 'HTMLAttributes<HTMLDivElement>'." | findstr /V /c:"JSX element class does not support attributes because it does not have a 'props' property." | findstr /V /c:"is not assignable to type 'HTMLAttributes<" | findstr /V /c:"does not exist on type 'HTMLAttributes<" | findstr /V /c:"is not assignable to type 'FunctionComponent<" & echo.