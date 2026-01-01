import React from 'react';

interface ButtonProps {
  onClick: () => void;
  children: React.ReactNode;
  variant?: 'primary' | 'secondary' | 'danger';
  disabled?: boolean;
}

const Button: React.FC<ButtonProps> = ({ onClick, children, variant = 'primary', disabled = false }) => {
  const baseStyle = "px-4 py-2 rounded-md font-semibold text-white";
  let variantStyle = "";

  switch (variant) {
    case 'primary':
      variantStyle = "bg-blue-600 hover:bg-blue-700";
      break;
    case 'secondary':
      variantStyle = "bg-gray-500 hover:bg-gray-600";
      break;
    case 'danger':
      variantStyle = "bg-red-600 hover:bg-red-700";
      break;
  }

  const disabledStyle = disabled ? "opacity-50 cursor-not-allowed" : "";

  return (
    <button
      className={`${baseStyle} ${variantStyle} ${disabledStyle}`}
      onClick={onClick}
      disabled={disabled}
    >
      {children}
    </button>
  );
};

export default Button;
