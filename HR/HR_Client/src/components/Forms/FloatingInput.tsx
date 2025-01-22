import { useState, useEffect } from 'react';

interface InputProps {
  label: string;
  incorrectLabel: string;
  type: string;
  regex?: RegExp;
  defaultColor?: string;
  value: string;
  onChange: React.ChangeEventHandler<HTMLInputElement>;
  autoComplete?: string;  // Added autoComplete prop
}

const FloatingLabelInput: React.FC<InputProps> = ({
  label,
  incorrectLabel,
  type,
  regex,
  defaultColor = 'blue',
  value,
  onChange,
}) => {
  const [focused, setFocused] = useState(false);
  const [hasValue, setHasValue] = useState(false);
  const [isValid, setIsValid] = useState(true);
  const [borderColor, setBorderColor] = useState<string>('gray-500');

  const handleFocus = () => {
    setFocused(true);
  };

  const handleBlur = (e: React.FocusEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setFocused(false);
    setHasValue(value.length > 0);

    if (regex) {
      setIsValid(regex.test(value));
    } else if (type === 'email') {
      setIsValid(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/i.test(value));
    } else if (type === 'password') {
      setIsValid(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{10,}$/.test(value));
    } else {
      setIsValid(true);
    }
  };

  const labelText = isValid ? label : incorrectLabel;

  const getColor = () => {
    if (focused) return `${defaultColor}-500`;
    if (hasValue) return isValid ? `${defaultColor}-500` : 'red-500';
    return 'gray-500';
  };

  useEffect(() => {
    setBorderColor(getColor());
  }, [focused, hasValue, isValid]);

  return (
    <div className="relative min-h-14 my-8 ">
      <input
        type={type}
        value={value}
        onChange={onChange}
        className={`
          block w-full px-3 py-2 transition-all duration-300 focus:outline-none focus:ring-0
          outline-none
          border-b-2
          border-${borderColor} 
          ${focused || hasValue ? 'pt-6 text-sm' : 'pt-3 text-base'}
        `}
        onFocus={handleFocus}
        onBlur={handleBlur}
      />

      <label
        className={`
          absolute left-3 top-1/2 transform transition-all duration-300
          ${focused || hasValue ? `text-sm text-${borderColor} -translate-y-5` : `text-base text-${borderColor} -translate-y-1/2`}
        `}
      >
        {labelText}
      </label>
    </div>
  );
};

export default FloatingLabelInput;
