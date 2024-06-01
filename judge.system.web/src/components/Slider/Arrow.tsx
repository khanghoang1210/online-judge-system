import React from 'react';
import { FaChevronLeft, FaChevronRight } from 'react-icons/fa';

const SampleNextArrow = (props:any) => {
  const { className, style, onClick } = props;
  return (
    <div
      className={`${className} custom-arrow next`}
      style={{ ...style, display: 'flex', right: '10px' }}
      onClick={onClick}
    >
      <FaChevronRight className="text-black" />
    </div>
  );
};

const SamplePrevArrow = (props:any) => {
  const { className, style, onClick } = props;
  return (
    <div
      className={`${className} custom-arrow prev`}
      style={{ ...style, display: 'block', left: '0px' }}
      onClick={onClick}
    >
      <FaChevronLeft className="text-black" />
    </div>
  );
};

export { SampleNextArrow, SamplePrevArrow };