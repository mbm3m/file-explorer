# Composition Design Pattern: File/Folder Traversal Application

## Overview

This project implements a .NET desktop application using the Composition Design Pattern to demonstrate file and folder traversal. The application allows users to select a folder, recursively traverse its contents (files and subfolders), and visualize the sizes of files and folders. The visualization can either be a **Bar-Chart** or a **Pie-Chart**.

The design follows the **Composite Design Pattern** to handle files and folders as part of a hierarchy, and the **Strategy Pattern** to enable flexible visualizations (Bar-Chart or Pie-Chart).

## Features

- **Folder Traversal:** Select a folder and recursively traverse all its files and subfolders.
- **Visualization:** Display the size of files and folders using Bar-Chart or Pie-Chart visualization.
  - **Bar-Chart:** Implemented manually (no external libraries).
  - **Pie-Chart:** Uses an external component.
- **Interactive UI:** 
  - Browse and select a folder.
  - View folder and file details.
  - Toggle between Bar-Chart and Pie-Chart visualizations.
- **Responsive Design:** The application adapts to changes in window size, showing scrollbars when necessary.

## Design Patterns

### Composite Design Pattern

- **Files:** A file is represented by a name, size, and extension.
- **Folders:** A folder contains similar attributes but lacks an extension. It can also contain a list of files or other subfolders.
- **Application Class:** This class manages the folder selection and visualization of the contents.

### Strategy Design Pattern

- **Visualization Strategy:** The strategy pattern is used to allow the visualization to switch between **Bar-Chart** and **Pie-Chart**. 
  - **Bar-Chart Strategy:** A concrete class for Bar-Chart visualization.
  - **Pie-Chart Strategy:** A concrete class for Pie-Chart visualization.
- **Context (Application):** The application that allows switching between different visualization strategies.

## Getting Started

1. **Clone the Repository:**
   - Clone this repository to your local machine using the following command:
     ```bash
     git clone <repository-url>
     ```

2. **Install .NET Framework:**
   - Ensure you have the required version of the .NET framework to run this application. The application is written in **Visual Basic (VB)** using the .NET desktop environment.

3. **Run the Application:**
   - Open the project in Visual Studio.
   - Press **F5** or click on the **Start** button to run the application.
   - The application will start with the default directory (C:\ drive). Use the **Browse Button** to select other folders for visualization.

4. **UI and Interaction:**
   - Upon launching the app, the folder contents are displayed in **Area A**.
   - Clicking a folder or subfolder will display its contents in **Area B**.
   - Toggle between **Bar-Chart** and **Pie-Chart** visualizations by selecting the corresponding option.
   - Resize the application window and observe responsive behavior with scrollbars when necessary.


## Requirements

1. **Folder Selection:** Allow users to select a folder via a **Browse** button.
2. **Visualization:**
   - Use **Bar-Chart** (manually implemented) or **Pie-Chart** (external component) to visualize folder and file sizes.
3. **Responsive Design:** The application should be responsive and adjust to window resizing. Display scrollbars when necessary.
4. **Switch Visualization Types:** Users can toggle between Bar-Chart and Pie-Chart views.

## Dependencies

- **.NET Framework**: Ensure you have a compatible version of the .NET Framework installed (for VB.NET desktop applications).
