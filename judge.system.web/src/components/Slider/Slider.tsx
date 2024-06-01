import React from 'react';
import Slider from 'react-slick';
import Image from 'next/image';
import Link from 'next/link';
import "slick-carousel/slick/slick.css"; 
import "slick-carousel/slick/slick-theme.css";
import { SampleNextArrow, SamplePrevArrow } from './Arrow';

const sliderItems = [
  {
    title: "Data Structures and Algorithms",
    chapters: 13,
    items: 149,
    progress: 0,
    imgSrc: "/img-problem1.png",
  
  },
  {
    title: "System Design for Interviews and Beyond",
    chapters: 16,
    items: 81,
    progress: 0,
    imgSrc: "/img-problem2.png",

  },
  {
    title: "The LeetCode Beginner's Guide",
    chapters: 4,
    items: 17,
    progress: 0,
    imgSrc: "/img-problem3.png",

  },
  {
    title: "Top Interview Questions",
    chapters: 9,
    items: 48,
    progress: 0,
    imgSrc: "/img-problem4.png",

  },{
    title: "The LeetCode Beginner's Guide",
    chapters: 4,
    items: 17,
    progress: 0,
    imgSrc: "/img-problem5.png",

  },
  {
    title: "Top Interview Questions",
    chapters: 9,
    items: 48,
    progress: 0,
    imgSrc: "/img-problem6.png",

  },
  // Add more items as needed
];

interface CustomSliderProps {
  title: string;
}

const CustomSlider: React.FC<CustomSliderProps> = ({ title }) => {
  const settings = {
    dots: true,
    infinite: true,
    speed: 500,
    slidesToShow: 5,
    slidesToScroll: 1,
    nextArrow: <SampleNextArrow />,
    prevArrow: <SamplePrevArrow />,
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1,
          infinite: true,
          dots: true,
        },
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
        },
      },
    ],
  };

  return (
    <>
      <div className='slider-container px-4 py-8'>
        <h2 className='text-3xl font-bold mb-4 text-white'>{title}</h2>
        <Slider {...settings}>
          {sliderItems.map((item, index) => (
            <div key={index} className='px-2' >
              <Link href={`postDetail?id=${index}`}   >
                <div className='text-3xl bg-white rounded-lg shadow-lg overflow-hidden m-0.5' style={{ height: "100%",position:"relative" }}>
                  <div style={{ position: "relative", height: "150px" }}>
                    <Image
                      src={item.imgSrc}
                      alt={item.title}
                      layout='fill'
                      objectFit='cover'
                      objectPosition='center'
                    />
                  </div>
                  <div className='p-4 text-black' >
                    <h3 className='text-2xl text-white font-bold mb-2'  style={{position:"absolute",top:"24px",left:"20px",right:"20px"}}>{item.title}</h3>
                    <div className='flex justify-between text-sm'>
                      <span>{item.chapters} Chapters</span>
                      <span>{item.items} Items</span>
                      <span>{item.progress}%</span>
                    </div>
                  </div>
                </div>
              </Link>
            </div>
          ))}
        </Slider>
      </div>
    </>
  );
};

export default CustomSlider;
