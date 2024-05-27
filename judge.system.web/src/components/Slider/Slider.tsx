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
    imgSrc: "https://image.nhandan.vn/Uploaded/2024/unqxwpejw/2023_09_24/anh-dep-giao-thong-1626.jpg",
    link: "/",
  },
  {
    title: "System Design for Interviews and Beyond",
    chapters: 16,
    items: 81,
    progress: 0,
    imgSrc: "https://image.nhandan.vn/Uploaded/2024/unqxwpejw/2023_09_24/anh-dep-giao-thong-1626.jpg",
    link: "/",
  },
  {
    title: "The LeetCode Beginner's Guide",
    chapters: 4,
    items: 17,
    progress: 0,
    imgSrc: "https://image.nhandan.vn/Uploaded/2024/unqxwpejw/2023_09_24/anh-dep-giao-thong-1626.jpg",
    link: "/",
  },
  {
    title: "Top Interview Questions",
    chapters: 9,
    items: 48,
    progress: 0,
    imgSrc: "https://image.nhandan.vn/Uploaded/2024/unqxwpejw/2023_09_24/anh-dep-giao-thong-1626.jpg",
    link: "/",
  },{
    title: "The LeetCode Beginner's Guide",
    chapters: 4,
    items: 17,
    progress: 0,
    imgSrc: "https://image.nhandan.vn/Uploaded/2024/unqxwpejw/2023_09_24/anh-dep-giao-thong-1626.jpg",
    link: "/",
  },
  {
    title: "Top Interview Questions",
    chapters: 9,
    items: 48,
    progress: 0,
    imgSrc: "https://image.nhandan.vn/Uploaded/2024/unqxwpejw/2023_09_24/anh-dep-giao-thong-1626.jpg",
    link: "/",
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
      <h2 className='text-2xl font-bold mb-4 text-white'>{title}</h2>
      <Slider {...settings}>
        {sliderItems.map((item, index) => (
          <div key={index} className='px-2' >
            <Link href={item.link}>
              <div className='bg-white rounded-lg shadow-lg overflow-hidden m-2' style={{ height: "100%" }}>
                <div style={{ position: "relative", height: "150px" }}>
                  <Image
                    src={item.imgSrc}
                    alt={item.title}
                    layout='fill'
                    objectFit='cover'
                    objectPosition='center'
                  />
                  <div className='p-4 absolute bottom-0 left-0 right-0 text-white'>
                    <h3 className='text-lg font-bold'>{item.title}</h3>
                    <div className='flex justify-between mt-2 text-sm'>
                      <span>{item.chapters} Chapters</span>
                      <span>{item.items} Items</span>
                      <span>{item.progress}%</span>
                    </div>
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
