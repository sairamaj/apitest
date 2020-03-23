"""Setup script for realpython-reader"""

import os.path
from setuptools import setup

# The directory containing this file
HERE = os.path.abspath(os.path.dirname(__file__))

# The text of the README file
with open(os.path.join(HERE, "README.md")) as fid:
    README = fid.read()

setup(
    name="apitester",
    version="1.0.0",
    description="To do api test through console",
    long_description=README,
    long_description_content_type="text/markdown",
    url="https://github.com/sairamaj/apitest",
    author="Sairama Jamalapuram",
    author_email="sairamaj@hotmail.com",
    license="MIT",
    classifiers=[
        "License :: OSI Approved :: MIT License",
        "Programming Language :: Python",
        "Programming Language :: Python :: 2",
        "Programming Language :: Python :: 3",
    ],
    packages=["reader"],
    include_package_data=True,
    install_requires=[
        "pywin32"
    ],
    entry_points={"console_scripts": ["apitester"]},
)